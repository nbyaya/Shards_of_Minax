using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a venom scorpion corpse")]
    public class VenomScorpion : BaseCreature
    {
        private DateTime m_NextVenomBurst;

        [Constructable]
        public VenomScorpion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venom scorpion";
            Body = 48;
            BaseSoundID = 397;
            Hue = 1267; // Distinct venomous hue

            SetStr(200);
            SetDex(100);
            SetInt(50);

            SetHits(150);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 85.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            PackItem(new GreaterPoisonPotion());

            m_NextVenomBurst = DateTime.UtcNow;
        }

        public VenomScorpion(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextVenomBurst)
            {
                VenomBurst();
            }
        }

        private void VenomBurst()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && CanBeHarmful(m))
                {
                    m.ApplyPoison(this, Poison.Deadly);
                    m.PlaySound(0x474);
                    m.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist);
                }
            }

            m_NextVenomBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
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

            m_NextVenomBurst = DateTime.UtcNow;
        }
    }
}
