using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an enigmatic skipper corpse")]
    public class EnigmaticSkipper : BaseCreature
    {
        private DateTime m_NextEtherealSkip;
        private DateTime m_NextMysticVeil;
        private DateTime m_NextRealityFracture;
        private DateTime m_NextTeleport;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public EnigmaticSkipper()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an enigmatic skipper";
            Body = 205; // Rabbit body
            Hue = 2252; // Unique hue

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

            m_AbilitiesInitialized = false;
        }

        public EnigmaticSkipper(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEtherealSkip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                    m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 25));
                    m_NextRealityFracture = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 30));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextEtherealSkip)
                {
                    EtherealSkip();
                }

                if (DateTime.UtcNow >= m_NextMysticVeil)
                {
                    MysticVeil();
                }

                if (DateTime.UtcNow >= m_NextRealityFracture)
                {
                    RealityFracture();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }
            }
        }

        private void EtherealSkip()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Skipper flickers in and out of visibility, evading attacks!*");
            PlaySound(0x1D2); // Ethereal sound effect

            this.VirtualArmor += 15; // Temporarily increase virtual armor

            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                this.VirtualArmor -= 15; // Revert virtual armor
            });

            m_NextEtherealSkip = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void MysticVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Skipper shrouds itself in a mystic veil, confusing your attacks!*");
            PlaySound(0x1D5); // Mystic sound effect

            // Apply debuff to enemies
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("Your attacks are less accurate due to the mystic veil!");
                    // Implement debuff (e.g., reduce hit chance)
                }
            }

            m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown
        }

        private void RealityFracture()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Skipper fractures reality, causing chaos in the area!*");
            PlaySound(0x307); // Explosion sound effect

            // Area-of-effect damage
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by a reality-shattering force!");
                    Effects.SendLocationEffect(m.Location, Map, 0x36BD, 20, 10); // Visual effect
                }
            }

            m_NextRealityFracture = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown
        }

        private void Teleport()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Skipper vanishes and reappears elsewhere!*");
            PlaySound(0x1E1); // Teleport sound effect

            // Teleport to a random location within a range
            int range = 10;
            int xOffset = Utility.RandomMinMax(-range, range);
            int yOffset = Utility.RandomMinMax(-range, range);
            Point3D newLocation = new Point3D(X + xOffset, Y + yOffset, Z);

            if (Map.CanSpawnMobile(newLocation) && Map.CanFit(newLocation, 16, false, false))
            {
                Location = newLocation;
            }

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown
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

            m_AbilitiesInitialized = false;
        }
    }
}
