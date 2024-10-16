using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an eldritch harbinger corpse")]
    public class EldritchHarbinger : BaseCreature
    {
        private DateTime m_NextRuneBlast;
        private DateTime m_NextHarbingersCall;
        private DateTime m_NextRealityWarp;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public EldritchHarbinger()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Eldritch Harbinger";
            Body = 205; // Rabbit body
            Hue = 2257; // Unique hue for mystical appearance

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

        public EldritchHarbinger(Serial serial)
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
                    m_NextRuneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHarbingersCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRealityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRuneBlast)
                {
                    RuneBlast();
                }

                if (DateTime.UtcNow >= m_NextHarbingersCall)
                {
                    HarbingersCall();
                }

                if (DateTime.UtcNow >= m_NextRealityWarp)
                {
                    RealityWarp();
                }
            }
        }

        private void RuneBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eldritch Harbinger releases a burst of eldritch energy from its runes! *");
            PlaySound(0x20F); // Magical explosion sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(50, 75), 0, 0, 100, 0, 0); // Massive eldritch damage
                    m.SendMessage("You are struck by a powerful blast of eldritch energy!");
                }
            }

            m_NextRuneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for RuneBlast
        }

        private void HarbingersCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eldritch Harbinger summons minor eldritch creatures! *");
            PlaySound(0x20F); // Summoning sound

            // Summon minor eldritch creatures (e.g., eldritch minions)
            for (int i = 0; i < 3; i++)
            {
                EldritchMinion minion = new EldritchMinion();
                minion.MoveToWorld(Location, Map);
                minion.Combatant = this;
            }

            m_NextHarbingersCall = DateTime.UtcNow + TimeSpan.FromSeconds(120); // Cooldown for HarbingersCall
        }

        private void RealityWarp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eldritch Harbinger twists reality around it! *");
            PlaySound(0x20F); // Distortion sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    Point3D newLocation = new Point3D(
                        Location.X + Utility.RandomMinMax(-10, 10),
                        Location.Y + Utility.RandomMinMax(-10, 10),
                        Location.Z
                    );

                    m.MoveToWorld(newLocation, Map);
                    m.SendMessage("You are suddenly teleported by a reality warp!");
                }
            }

            m_NextRealityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Cooldown for RealityWarp
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

    public class EldritchMinion : BaseCreature
    {
        [Constructable]
        public EldritchMinion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Eldritch Minion";
            Body = 205; // Rabbit body
            Hue = 1155; // Different hue for minions

            SetStr(50);
            SetDex(50);
            SetInt(50);

            SetHits(50);
            SetMana(50);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 10);
            SetResistance(ResistanceType.Energy, 30);

            SetSkill(SkillName.MagicResist, 30.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;

            Tamable = false;
        }

        public EldritchMinion(Serial serial)
            : base(serial)
        {
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
        }
    }
}
