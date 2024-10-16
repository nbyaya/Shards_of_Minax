using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a banshee crab corpse")]
    public class BansheeCrab : CoconutCrab
    {
        private DateTime m_NextWailingPull;
        private DateTime m_NextScreechAttack;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BansheeCrab()
            : base("Banshee Crab")
        {
            Hue = 1461; // Ghostly hue
            Name = "a Banshee Crab";
			BaseSoundID = 0x4F2;
			
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
            SetResistance(ResistanceType.Poison, 100);
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

        public BansheeCrab(Serial serial)
            : base(serial)
        {
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
                    m_NextWailingPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextScreechAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWailingPull)
                {
                    WailingPull();
                }

                if (DateTime.UtcNow >= m_NextScreechAttack)
                {
                    ScreechAttack();
                }
            }
        }

        private void WailingPull()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Wails mournfully and pulls you in! *");

            Mobile target = Combatant as Mobile;
            if (target != null)
            {
                target.Freeze(TimeSpan.FromSeconds(2)); // Inflict fear by freezing

                Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(() =>
                {
                    if (target != null && !target.Deleted)
                    {
                        Point3D newLocation = GetSpawnPosition(10); // Teleport target away
                        if (newLocation != Point3D.Zero)
                        {
                            target.MoveToWorld(newLocation, Map);
                            target.SendMessage("You have been pulled away by a ghostly force!");
                        }
                    }
                }));
            }

            m_NextWailingPull = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void ScreechAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Screeches loudly, inflicting terror! *");

            Mobile target = Combatant as Mobile;
            if (target != null && target.Frozen)
            {
                int damage = Utility.RandomMinMax(10, 20); // Additional damage if target is in fear
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.PlaySound(0x1F5); // Screech sound
            }

            m_NextScreechAttack = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
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

            // Reset the initialization flag and set random intervals on deserialization
            m_AbilitiesInitialized = false; // Ensure abilities are reinitialized on load
        }
    }
}
