using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei.Character
{
    using EntityForm = IEntityForm<IEnumerable<Stat>, Stats>;

    public class CharacterStatsHandler
    {
        public CharacterStatsHandler(
            EntityForm         characterStatsForm,
            DomainEventService service,
            object             group = null)
        {
            Group          = group;
            StatsForm      = characterStatsForm;
            CalculateStats = new();

            EventRegister(service);
        }

        public object                          Group          { get; }
        public EntityForm                      StatsForm      { get; protected set; }
        public Dictionary<int, CalculateStats> CalculateStats { get; }

        public CalculateStat this[int guidHash, string stat]
        {
            get
            {
                var exist = CalculateStats.TryGetValue(guidHash, out var stats);

                return exist ? stats.SearchAt(stat).To<CalculateStat>() : new CalculateStat(stat, 0 ,0);
            }
        }

        public CalculateStats this[int guidHash]
            => CalculateStats.GetorReturn(guidHash, () => new CalculateStats("", new CalculateStat[0]));

        protected virtual void EventRegister(DomainEventService service)
        {
            service.Register<CharacterStatsCreate> (this.StatsCreate          , Group);
            service.Register<CharacterStatsRelease>(this.StatsRelease         , Group);
            service.Register<CalculateStatIncrease>(this.CalculateStatIncrease, Group);
            service.Register<CalculateStatDecrease>(this.CalculateStatDecrease, Group);
            service.Register<StandardStatIncrease> (this.StandardStatIncrease , Group);
            service.Register<StandardStatDecrease> (this.StandardStatDecrease , Group);
            service.Register<CharacterStatFetch>   (this.StatFetch            , Group);
        }

        public virtual void StatsCreate(object characterId, int guidHash) 
        {
            var stats = StatsForm[characterId]
                .To<IExtract>()
                .Extract()
                .To<CalculateStats>();
            
            CalculateStats.Add(guidHash, stats);
        }

        public virtual void StatsRelease(int guidHash) 
        {
            CalculateStats.Remove(guidHash);
        }

        public virtual VariableResponse CalculateStatIncrease(int guidHash, string statName, float variable) 
        {
            var stat = this[guidHash, statName];

            stat.Increase(variable);

            return new(stat);
        }

        public virtual VariableResponse CalculateStatDecrease(int guidHash, string statName, float variable)
        {
            var stat = this[guidHash, statName];

            stat.Decrease(variable);

            return new(stat);
        }

        public virtual VariableResponse StandardStatIncrease(int guidHash, string statName, float variable)
        {
            var stat = this[guidHash, statName];

            stat.Reset(stat.Standard + variable);
            
            return new(stat);
        }

        public virtual VariableResponse StandardStatDecrease(int guidHash, string statName, float variable)
        {
            var stat = this[guidHash, statName];

            stat.Reset(stat.Standard - variable);

            return new(stat);
        }

        public virtual VariableResponse StatFetch(int guidHash, string statName) 
        {
            var stat = this[guidHash, statName];

            return new(stat);
        }
    }

    public class VariableResponse : ResponseBase 
    {
        public VariableResponse (CalculateStat stat) 
            : this(stat.Identity, stat.Amount, stat.Data, stat.Standard)
        {

        }

        public VariableResponse (object statId, float variable, float value, float standard)
        {
            StatId   = statId;
            Variable = variable;
            Value    = value;
            Standard = standard;
        }

        public object StatId   { get; }
        public float  Variable { get; }
        public float  Value    { get; }
        public float  Standard { get; }
    }
}