using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Custom
{
    public class SilverSlime : BaseCreature
    {
        [Constructable]
        public SilverSlime() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a SilverSlime";
            Body = 51;
            BaseSoundID = 456;
			Team = Utility.RandomMinMax(1, 5);

            SetStr(22, 34);
            SetDex(16, 21);
            SetInt(16, 20);

            SetHits(100, 900);

            SetDamage(1, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 10.0);
            SetSkill(SkillName.Magery, 10.0);
            SetSkill(SkillName.MagicResist, 15.0);
            SetSkill(SkillName.Tactics, 10.0);
            SetSkill(SkillName.Wrestling, 10.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 8;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != null && !willKill && amount > 0 && from != this)
            {
                // Absorb any spells cast on the SilverSlime and cast Harm back to the caster
                if (from is BaseCreature && ((BaseCreature)from).Summoned)
                {
                    from.Damage(amount, this);
                    this.Hits += amount;
                    this.Mana += amount;
                    this.Stam += amount;
                    this.Say("Slurp!");
                    this.DoHarm(from);
                }
                else if (from is Mobile && from.Spell != null)
                {
                    from.Damage(amount, this);
                    this.Hits += amount;
                    this.Mana += amount;
                    this.Stam += amount;
                    this.Say("Slurp!");
                    this.DoHarm(from);
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public void DoHarm(Mobile target)
        {
            // Cast Harm spell on the target
            if (target != null && target.Alive && this.CanSee(target) && this.InLOS(target))
            {
                this.Say("Harm!");
                this.MovingParticles(target, 0x36E4, 5, 0, false, true, 3006, 4006, 0);
                target.PlaySound(0x1F1);
                SpellHelper.Damage(TimeSpan.Zero, target, this, Utility.RandomMinMax(6, 12));
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker != null && attacker.Alive && 0.1 > Utility.RandomDouble())
            {
                // Spawn a new SilverSlime when attacked
                this.Say("Split!");
                SilverSlime spawn = new SilverSlime();
                spawn.MoveToWorld(this.Location, this.Map);
                spawn.Combatant = attacker;
            }
        }

        public SilverSlime(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
