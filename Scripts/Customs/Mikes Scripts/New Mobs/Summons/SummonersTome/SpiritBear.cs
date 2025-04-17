using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.CustomSummons
{
    [CorpseName("a spirit bear corpse")]
    public class SpiritBear : BaseCreature
    {
        private Mobile _caster;

        [Constructable]
        public SpiritBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6)
        {

            Name = "a spirit bear";
            Body = 213;
            Hue = 0x47E; // Ethereal ghostly hue
            BaseSoundID = 0xA3;


            double scale = 0;

            SetStr((int)(200 + scale * 150), (int)(250 + scale * 150));
            SetDex((int)(100 + scale * 80), (int)(120 + scale * 80));
            SetInt((int)(50 + scale * 30));

            SetHits((int)(300 + scale * 200), (int)(350 + scale * 250));
            SetMana(0);

            SetDamage((int)(18 + scale * 10), (int)(24 + scale * 12));
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Cold, 20);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Energy, 25, 40);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Fire, 20, 35);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 4000;
            Karma = 0;

            VirtualArmor = 40;

            ControlSlots = 2;
            Tamable = false;

            // Unique abilities timer
            Timer.DelayCall(TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(20), GuardianRoar);
        }

        public override bool DeleteCorpseOnDeath => true;
        public override bool BardImmune => true;
        public override bool Unprovokable => true;
        public override bool AreaPeaceImmune => true;
        public override bool BleedImmune => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Utility.RandomDouble() < 0.05)
                SpiritMaul(Combatant as Mobile);
        }

        // Powerful attack with chance to stun
        private void SpiritMaul(Mobile target)
        {
            if (target == null || !target.Alive || target.Hidden)
                return;

            this.Say("*Spirit Bear mauls its foe with ethereal force!*");
            target.PlaySound(0x213);
            target.Damage(30, (Mobile)this);

            if (Utility.RandomDouble() < 0.3)
            {
                target.Freeze(TimeSpan.FromSeconds(2));
                target.SendMessage("You are stunned by the Spirit Bear's mighty blow!");
            }
        }

        // Defensive buff to nearby allies
        private void GuardianRoar()
        {
            if (this.Deleted || !this.Alive)
                return;

            this.Say("*The Spirit Bear roars, empowering nearby allies*");

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m == this || (m is BaseCreature c && c.ControlMaster == _caster))
                {
                    m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    m.PlaySound(0x20F);
                    m.SendMessage("You feel shielded by the Spirit Bear's energy.");
                    Buff(m);
                }
            }
        }

        private void Buff(Mobile m)
        {
            m.AddStatMod(new StatMod(StatType.Str, "SpiritBearStr", 10, TimeSpan.FromSeconds(10)));
            m.AddStatMod(new StatMod(StatType.Dex, "SpiritBearDex", 10, TimeSpan.FromSeconds(10)));
            m.AddStatMod(new StatMod(StatType.Int, "SpiritBearInt", 10, TimeSpan.FromSeconds(10)));
        }

        public SpiritBear(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
