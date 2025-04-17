using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an icebound elemental corpse")]
    public class IceboundElemental : BaseCreature, IAuraCreature
    {
        private Mobile _summoner;
        private double _spiritSpeakBonus;

        public override bool DeleteCorpseOnDeath => true;
        public override bool AlwaysMurderer => true;

        [Constructable]
        public IceboundElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an icebound elemental";
            Body = 161;
            BaseSoundID = 268;
            Hue = 1152; // Icy Blue

            _spiritSpeakBonus = 0;

            int bonus = 0;

            SetStr(185 + bonus);
            SetDex(110 + bonus / 2);
            SetInt(192 + bonus);

            SetHits(160 + (bonus * 2));
            SetDamage(15 + (bonus / 5), 25 + (bonus / 4));

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 40 + bonus / 10, 50 + bonus / 10);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.EvalInt, 60.0 + bonus / 2);
            SetSkill(SkillName.Magery, 60.0 + bonus / 2);
            SetSkill(SkillName.MagicResist, 90.0 + bonus / 2);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50 + bonus / 5;

            SetAreaEffect(AreaEffect.AuraDamage);

            PackItem(new BlackPearl());
        }

        public IceboundElemental(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (_summoner != null && !Deleted && !Summoned)
            {
                if (GetDistanceToSqrt(_summoner.Location) <= 6)
                {
                    this.VirtualArmor = 80; // Buff armor when near summoner
                }
                else
                {
                    this.VirtualArmor = 50;
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.2 > Utility.RandomDouble()) // 20% freeze chance
            {
                defender.Freeze(TimeSpan.FromSeconds(2.0));
                defender.SendMessage("You are frozen by the Icebound Elemental's chilling strike!");
                defender.FixedParticles(0x376A, 9, 32, 5005, 1152, 0, EffectLayer.Waist);
            }
        }

        public void AuraEffect(Mobile m)
        {
            m.FixedParticles(0x374A, 10, 30, 5052, Hue, 0, EffectLayer.Waist);
            m.PlaySound(0x5C6);
            m.SendMessage("You are chilled by the Icebound Elemental's frost aura!");
            AOS.Damage(m, this, Utility.RandomMinMax(6, 12), 0, 100, 0, 0, 0); // Cold-only aura
        }

        public override void GenerateLoot()
        {
            // No loot; summoned creature
        }

        public override bool BleedImmune => true;
        public override bool Unprovokable => true;
        public override bool BardImmune => true;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write(_summoner);
            writer.Write(_spiritSpeakBonus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                _summoner = reader.ReadMobile();
                _spiritSpeakBonus = reader.ReadDouble();
            }
        }
    }
}
