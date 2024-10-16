using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a mystic ferret corpse")]
    public class MysticFerret : BaseCreature
    {
        private DateTime m_NextEnchantedPuff;
        private DateTime m_NextMysticWard;
        private DateTime m_NextAuraBurst;
        private DateTime m_NextTeleport;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MysticFerret()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic ferret";
            Body = 0x117; // Ferret body
            Hue = 1571; // Rainbow-like hue
			BaseSoundID = 0xCF;

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

        public MysticFerret(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextEnchantedPuff = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMysticWard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextAuraBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEnchantedPuff)
                {
                    EnchantedPuff();
                }

                if (DateTime.UtcNow >= m_NextMysticWard)
                {
                    MysticWard();
                }

                if (DateTime.UtcNow >= m_NextAuraBurst)
                {
                    AuraBurst();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }
            }
        }

        private void EnchantedPuff()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Ferret blows a magical puff! *");
            Effects.SendLocationParticles(this, 0x373A, 10, 16, 5025);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m.Alive && m != this)
                {
                    int effect = Utility.Random(3);
                    switch (effect)
                    {
                        case 0:
                            m.Hits = Math.Min(m.Hits + 30, m.HitsMax);
                            m.SendMessage("You feel a soothing energy heal your wounds!");
                            break;
                        case 1:
                            m.SendMessage("You feel a surge of power!");
                            break;
                        case 2:
                            m.SendMessage("You feel a regenerative aura surrounding you!");
                            break;
                    }
                }
            }

            m_NextEnchantedPuff = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for EnchantedPuff
        }

        private void MysticWard()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Ferret creates a mystical barrier! *");
            Effects.SendLocationParticles(this, 0x373A, 10, 16, 5025);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m.Alive && m != this)
                {
                    m.SendMessage("A mystical barrier surrounds you, absorbing some of the damage!");
                }
            }

            m_NextMysticWard = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for MysticWard
        }

        private void AuraBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Ferret emits a burst of magical aura! *");
            Effects.SendLocationParticles(this, 0x376A, 10, 16, 5025);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int effect = Utility.Random(2);
                    switch (effect)
                    {
                        case 0:
                            m.SendMessage("You are momentarily disoriented by the aura!");
                            break;
                        case 1:
                            m.SendMessage("You feel weakened by the aura!");
                            m.SendMessage("You take extra damage from physical attacks!");
                            break;
                    }
                }
            }

            m_NextAuraBurst = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for AuraBurst
        }

        private void Teleport()
        {
            if (Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(10);
                if (newLocation != Point3D.Zero && !newLocation.Equals(Location))
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Ferret vanishes in a puff of smoke! *");
                    Effects.SendLocationParticles(this, 0x3709, 10, 30, 5025);
                    MoveToWorld(newLocation, Map);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Ferret reappears at a new location! *");
                }
            }

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Teleport
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
