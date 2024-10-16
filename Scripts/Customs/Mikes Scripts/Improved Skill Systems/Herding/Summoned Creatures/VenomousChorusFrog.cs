using System;
using System.Collections.Generic; // Add this line
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a venomous chorus frog corpse")]
    public class VenomousChorusFrog : BaseCreature
    {
        private DateTime m_NextToxicCroak;

        [Constructable]
        public VenomousChorusFrog()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous chorus frog";
            Body = 81;
            Hue = 1272; // Vibrant green hue
            BaseSoundID = 0x266;

            SetStr(80, 100);
            SetDex(70, 90);
            SetInt(100, 120);

            SetHits(100, 120);
            SetMana(40, 60);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 60.1, 70.0);
            SetSkill(SkillName.Tactics, 70.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 70.0);
            SetSkill(SkillName.Poisoning, 80.1, 90.0);
            SetSkill(SkillName.Magery, 70.1, 80.0);
            SetSkill(SkillName.EvalInt, 70.1, 80.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 36;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextToxicCroak = DateTime.UtcNow;
        }

        public VenomousChorusFrog(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 4; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat; } }
        public override Poison HitPoison { get { return Poison.Greater; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextToxicCroak)
            {
                ToxicCroak();
            }
        }

        private void ToxicCroak()
        {
            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && CanBeHarmful(m) && !m.Hidden)
                {
                    targets.Add(m);
                }
            }

            if (targets.Count > 0)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The frog releases a toxic croak! *");
                PlaySound(0x266);

                foreach (Mobile target in targets)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    target.ApplyPoison(this, Poison.Greater);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);

                    target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                    target.PlaySound(0x474);
                }

                m_NextToxicCroak = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }
		
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextToxicCroak = DateTime.UtcNow;
        }
    }
}
