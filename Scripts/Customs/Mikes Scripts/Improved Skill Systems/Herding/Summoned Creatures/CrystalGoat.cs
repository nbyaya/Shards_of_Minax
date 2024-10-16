using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a crystal goat corpse")]
    public class CrystalGoat : BaseCreature
    {
        private DateTime m_NextCrystallineShield;
        private DateTime m_CrystallineShieldEnd;

        [Constructable]
        public CrystalGoat()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a crystal goat";
            this.Body = 88; // Use the body type from the provided example
            this.BaseSoundID = 0xDB;
            this.Hue = 1153; // Blue hue

            this.SetStr(150);
            this.SetDex(80);
            this.SetInt(60);

            this.SetHits(120);
            this.SetMana(0);

            this.SetDamage(10, 15);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 35);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 15, 25);

            this.SetSkill(SkillName.MagicResist, 50.0, 60.0);
            this.SetSkill(SkillName.Tactics, 60.0, 75.0);
            this.SetSkill(SkillName.Wrestling, 60.0, 75.0);

            this.Fame = 450;
            this.Karma = 0;

            this.VirtualArmor = 30;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;

            m_NextCrystallineShield = DateTime.UtcNow;
        }

        public CrystalGoat(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextCrystallineShield && DateTime.UtcNow >= m_CrystallineShieldEnd)
            {
                ActivateCrystallineShield();
            }

            if (DateTime.UtcNow >= m_CrystallineShieldEnd && m_CrystallineShieldEnd != DateTime.MinValue)
            {
                DeactivateCrystallineShield();
            }
        }

        private void ActivateCrystallineShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Crystalline Shield! *");
            PlaySound(0x1ED);
            FixedEffect(0x376A, 10, 16);

            SetResistance(ResistanceType.Physical, 75, 85);

            m_CrystallineShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextCrystallineShield = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void DeactivateCrystallineShield()
        {
            SetResistance(ResistanceType.Physical, 25, 35);

            m_CrystallineShieldEnd = DateTime.MinValue;
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

            m_NextCrystallineShield = DateTime.UtcNow;
            m_CrystallineShieldEnd = DateTime.MinValue;
        }

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
    }
}
