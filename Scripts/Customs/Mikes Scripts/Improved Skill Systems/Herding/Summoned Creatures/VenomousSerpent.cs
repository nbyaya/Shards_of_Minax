using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a venomous serpent corpse")]
    public class VenomousSerpent : BaseCreature
    {
        private DateTime m_NextVenomousBurst;

        [Constructable]
        public VenomousSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous serpent";
            Body = 52; // Using the same body type as the provided example
            Hue = 1267; // Unique hue for this special snake
            BaseSoundID = 0xDB;

            this.SetStr(150);
            this.SetDex(100);
            this.SetInt(50);

            this.SetHits(120);
            this.SetMana(0);

            this.SetDamage(10, 15);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Poison, 50);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.Poisoning, 80.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 70.1, 80.0);
            this.SetSkill(SkillName.Tactics, 90.0);
            this.SetSkill(SkillName.Wrestling, 85.1, 90.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 34;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = -10;

            m_NextVenomousBurst = DateTime.UtcNow;
        }

        public VenomousSerpent(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextVenomousBurst)
            {
                VenomousBurst();
            }
        }

        private void VenomousBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Venomous Burst! *");
            PlaySound(0x1FB);
            FixedEffect(0x37B9, 10, 25);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                    m.ApplyPoison(this, Poison.Deadly);
                }
            }

            m_NextVenomousBurst = DateTime.UtcNow + TimeSpan.FromMinutes(5);
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

            m_NextVenomousBurst = DateTime.UtcNow;
        }
    }
}
