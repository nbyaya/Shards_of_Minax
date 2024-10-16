using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a shadow muntjac corpse")]
    public class ShadowMuntjac : BaseCreature
    {
        private DateTime m_NextShadowMeld;
        private DateTime m_NextSilentStrike;
        private DateTime m_NextShadowClone;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowMuntjac()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow muntjac";
            Body = 0xEA; // Using GreatHart body
            Hue = 1989; // Dark hue to blend into shadows

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

        public ShadowMuntjac(Serial serial)
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
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextShadowMeld = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random start for ShadowMeld
                    m_NextSilentStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25)); // Random start for SilentStrike
                    m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random start for ShadowClone
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowMeld)
                {
                    ShadowMeld();
                }

                if (DateTime.UtcNow >= m_NextSilentStrike)
                {
                    SilentStrike();
                }

                if (DateTime.UtcNow >= m_NextShadowClone)
                {
                    ShadowClone();
                }
            }
        }

        private void ShadowMeld()
        {
            PublicOverheadMessage(0, 1152, false, "* The Shadow Muntjac disappears into the shadows!*");

            // Make the Muntjac invisible and reappear behind the enemy
            if (Combatant != null)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(delegate
                {
                    if (Combatant != null && !Deleted)
                    {
                        Point3D newLocation = Combatant.Location;
                        newLocation.X += Utility.RandomMinMax(-2, 2);
                        newLocation.Y += Utility.RandomMinMax(-2, 2);

                        if (Map.CanSpawnMobile(newLocation))
                        {
                            MoveToWorld(newLocation, Map);
                            if (Combatant is Mobile)
                            {
                                ((Mobile)Combatant).SendMessage("The Shadow Muntjac reappears behind you!");
                            }
                            // Deliver a critical hit
                            Combatant.Damage(Utility.RandomMinMax(15, 25), this);
                        }
                    }
                }));
            }

            m_NextShadowMeld = DateTime.UtcNow + TimeSpan.FromSeconds(30.0); // Fixed cooldown for ShadowMeld
        }

        private void SilentStrike()
        {
            PublicOverheadMessage(0, 1152, false, "* The Shadow Muntjac strikes silently from the darkness!*");

            if (Combatant != null)
            {
                int damage = Utility.RandomMinMax(8, 15);
                if (Combatant is Mobile)
                {
                    ((Mobile)Combatant).SendMessage("You are bleeding from a silent strike!");
                }
                Combatant.Damage(damage, this);

                // Apply a bleed effect
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(delegate
                {
                    if (Combatant != null && !Combatant.Deleted)
                    {
                        if (Combatant is Mobile)
                        {
                            ((Mobile)Combatant).SendMessage("The bleeding from the Shadow Muntjac's strike continues!");
                        }
                        Combatant.Damage(damage / 2, this);
                    }
                }));
            }

            m_NextSilentStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20.0); // Fixed cooldown for SilentStrike
        }

        private void ShadowClone()
        {
            PublicOverheadMessage(0, 1152, false, "* The Shadow Muntjac creates a shadowy clone!*");

            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                ShadowClone clone = new ShadowClone(this);
                clone.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
                {
                    if (!clone.Deleted)
                        clone.Delete();
                }));

                m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown for ShadowClone
            }
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

    public class ShadowClone : BaseCreature
    {
        private Mobile m_Master;

        public ShadowClone(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);

            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public ShadowClone(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
