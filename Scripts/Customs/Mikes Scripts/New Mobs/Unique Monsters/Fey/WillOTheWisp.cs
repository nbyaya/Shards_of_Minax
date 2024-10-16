using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a will-o'-the-wisp corpse")]
    public class WillOTheWisp : BaseCreature
    {
        private DateTime m_NextLure;
        private DateTime m_NextTeleport;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public WillOTheWisp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a will-o'-the-wisp";
            Body = 58; // GreenGoblin body
            BaseSoundID = 466; // Wisp sound
            Hue = 1579; // Pale blue hue

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

            m_AbilitiesInitialized = false; // Set the initialization flag
        }

        public WillOTheWisp(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextLure = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random start for Lure
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25)); // Random start for Teleport
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLure)
                {
                    Lure();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }
            }
        }

        private void Lure()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The wisp's light becomes mesmerizing *");

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player && !m.Hidden)
                {
                    m.SendLocalizedMessage(502136); // You feel yourself being drawn away by the light...

                    Point3D newLocation = GetRandomNearbyLocation(m);
                    m.MoveToWorld(newLocation, Map);

                    m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    m.PlaySound(0x1FE);
                }
            }

            m_NextLure = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Lure
        }

        private void Teleport()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The wisp flickers and vanishes *");

            Point3D newLocation = GetRandomNearbyLocation(this);
            MoveToWorld(newLocation, Map);

            FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            PlaySound(0x1FE);

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Teleport
        }

        private Point3D GetRandomNearbyLocation(Mobile m)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = m.X + Utility.RandomMinMax(-3, 3);
                int y = m.Y + Utility.RandomMinMax(-3, 3);
                int z = m.Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (m.Map.CanSpawnMobile(p))
                    return p;
            }

            return m.Location;
        }

        public override void OnDelete()
        {
            base.OnDelete();
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
        }
    }
}
