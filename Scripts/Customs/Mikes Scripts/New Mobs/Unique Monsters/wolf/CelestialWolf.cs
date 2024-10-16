using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a celestial wolf corpse")]
    public class CelestialWolf : DireWolf
    {
        private DateTime m_NextDivineBite;
        private DateTime m_NextCelestialAura;
        private DateTime m_NextStarfall;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CelestialWolf()
            : base()
        {
            Name = "a celestial wolf";
            Body = 23; // DireWolf body
            Hue = 2639; // Custom celestial hue (light blue/white)
			BaseSoundID = 0xE5;
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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public CelestialWolf(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextDivineBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCelestialAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStarfall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextDivineBite)
                {
                    DivineBite();
                }

                if (DateTime.UtcNow >= m_NextCelestialAura)
                {
                    CelestialAura();
                }

                if (DateTime.UtcNow >= m_NextStarfall)
                {
                    Starfall();
                }
            }
        }

        private void DivineBite()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Celestial Bite! *");
            int holyDamage = Utility.RandomMinMax(10, 20);
            Combatant.Damage(holyDamage, this);

            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                Hits = Math.Min(Hits + holyDamage / 2, HitsMax);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Celestial healing radiates! *");
            }

            m_NextDivineBite = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void CelestialAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Celestial Aura! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Celestial Wolf's aura envelops you, reducing damage!");
                    if (m is PlayerMobile player)
                    {
                        player.VirtualArmor += 5; // Example effect, adjust as needed
                    }
                }
            }

            m_NextCelestialAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void Starfall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Starfall! *");
            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);
                    m.SendMessage("Celestial energy from the Starfall scorches you!");
                }

                if (m.Player)
                {
                    m.SendMessage("You feel the warmth of the celestial energy boosting your strength!");
                }
            }

            m_NextStarfall = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
