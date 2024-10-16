using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a smallpox rat corpse")]
    public class SmallpoxRat : GiantRat
    {
        private DateTime m_NextAbilityUse;
        private DateTime m_NextCallForHelp;
        private int m_HpThresholdForHelp = 30; // Health percentage threshold to call for help

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SmallpoxRat()
            : base()
        {
            Name = "a smallpox rat";
            Hue = 2263; // Set a unique hue for the Smallpox Rat
			this.BaseSoundID = 0xCC;
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

        public SmallpoxRat(Serial serial)
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
                    m_NextAbilityUse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCallForHelp = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAbilityUse)
                {
                    UsePoxMark();
                    m_NextAbilityUse = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Ability cooldown
                }

                if (DateTime.UtcNow >= m_NextCallForHelp && Hits <= (HitsMax * m_HpThresholdForHelp / 100))
                {
                    CallForHelp();
                    m_NextCallForHelp = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Call for help cooldown
                }
            }
        }

        private void UsePoxMark()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                // Apply Pox Mark effect
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Smallpox boils erupt on your skin! *");
                target.SendMessage("You feel the pox mark spreading!");

                // Apply initial damage and ongoing damage
                Timer.DelayCall(TimeSpan.FromSeconds(0), () =>
                {
                    ApplyPoxMark(target);
                });
            }
        }

        private void ApplyPoxMark(Mobile target)
        {
            // Apply initial damage
            int initialDamage = Utility.RandomMinMax(10, 20);
            target.Damage(initialDamage, this);
            target.SendMessage("You have been struck by a pox mark!");

            // Apply ongoing damage and reduce healing effectiveness
            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                if (target.Alive)
                {
                    int damageOverTime = Utility.RandomMinMax(5, 10);
                    target.Damage(damageOverTime, this);
                    target.SendMessage("The pox mark continues to spread!");
                }
            });

            // Apply visual effect
            target.FixedEffect(0x373A, 10, 16);
        }

        private void CallForHelp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Smallpox Rat squeaks loudly for reinforcements! *");

            // Spawn additional rats
            for (int i = 0; i < 2; i++)
            {
                Point3D loc = GetSpawnPosition(2);
                if (loc != Point3D.Zero)
                {
                    SmallpoxRat reinforcement = new SmallpoxRat();
                    reinforcement.MoveToWorld(loc, Map);
                    reinforcement.Combatant = Combatant;
                }
            }

            // Add sound effect
            PlaySound(0x2D9);
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
