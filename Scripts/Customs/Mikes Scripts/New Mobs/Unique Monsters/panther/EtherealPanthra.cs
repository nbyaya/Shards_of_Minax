using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ethereal panthra corpse")]
    public class EtherealPanthra : BaseCreature
    {
        private DateTime m_NextEtherealSwipe;
        private DateTime m_NextRealityRift;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EtherealPanthra()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ethereal panthra";
            Body = 0xD6; // Panther body
            Hue = 2183; // Ethereal blue hue
            BaseSoundID = 0x462; // Panther sound

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public EtherealPanthra(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEtherealSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRealityRift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEtherealSwipe)
                {
                    EtherealSwipe();
                }

                if (DateTime.UtcNow >= m_NextRealityRift)
                {
                    RealityRift();
                }
            }
        }

        private void EtherealSwipe()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ethereal claws materialize *");
            PlaySound(0x56D);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x1E1);

                    int damage = Utility.RandomMinMax(25, 35);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    m.Mana -= damage;
                    Mana += damage;

                    if (m.Player)
                        m.SendLocalizedMessage(1070848); // The ethereal panthra's attack drains your energy!
                }
            }

            Random rand = new Random();
            m_NextEtherealSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 45)); // Random cooldown for EtherealSwipe
        }

        private void RealityRift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Reality tears apart *");
            PlaySound(0x217);

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x213);

                    switch (Utility.Random(5))
                    {
                        case 0: // Damage
                            AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 0, 0, 0, 0, 100);
                            break;
                        case 1: // Poison
                            m.ApplyPoison(this, Poison.Greater);
                            break;
                        case 2: // Paralyze
                            m.Freeze(TimeSpan.FromSeconds(5));
                            break;
                        case 3: // Teleport
                            Point3D loc = GetRandomNearbyLocation(m.Map, 10);
                            m.MoveToWorld(loc, m.Map);
                            break;
                        case 4: // Mana Drain
                            int drain = Utility.RandomMinMax(20, 30);
                            m.Mana -= drain;
                            Mana += drain;
                            break;
                    }

                    if (m.Player)
                        m.SendLocalizedMessage(1070847); // The reality rift causes unpredictable effects!
                }
            }

            Random rand = new Random();
            m_NextRealityRift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Random cooldown for RealityRift
        }

        private Point3D GetRandomNearbyLocation(Map map, int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = map.GetAverageZ(x, y);

                if (map.CanSpawnMobile(x, y, z))
                    return new Point3D(x, y, z);
            }

            return Location;
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
