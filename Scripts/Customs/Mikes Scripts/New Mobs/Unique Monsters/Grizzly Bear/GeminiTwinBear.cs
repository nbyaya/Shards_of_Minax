using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a gemini twinbear corpse")]
    public class GeminiTwinBear : BaseCreature
    {
        private DateTime m_NextCloneTime;
        private DateTime m_NextDodgeTime;
        private DateTime m_NextQuakeTime;
        private DateTime m_NextFrenzyTime;
        private bool m_IsInFrenzy;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GeminiTwinBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Gemini TwinBear";
            Body = 212; // GrizzlyBear body
            Hue = 2001; // Unique hue
			BaseSoundID = 0xA3;

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

            m_AbilitiesInitialized = false; // Set flag to false
        }

        public GeminiTwinBear(Serial serial)
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
                    m_NextCloneTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextDodgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_NextFrenzyTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 180));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCloneTime)
                {
                    PerformCloneIllusion();
                }

                if (DateTime.UtcNow >= m_NextDodgeTime)
                {
                    QuickWit();
                }

                if (DateTime.UtcNow >= m_NextQuakeTime)
                {
                    Earthquake();
                }

                if (DateTime.UtcNow >= m_NextFrenzyTime && !m_IsInFrenzy)
                {
                    EnterFrenzyMode();
                }
            }
        }

        private void PerformCloneIllusion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini TwinBear splits into two!*");

            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                GeminiTwinBear clone = new GeminiTwinBear();
                clone.MoveToWorld(loc, Map);
                clone.Combatant = this.Combatant;

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
                {
                    if (!clone.Deleted)
                        clone.Delete();
                }));
            }

            Random rand = new Random();
            m_NextCloneTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 120)); // Randomize cooldown
        }

        private void QuickWit()
        {
            if (Utility.RandomDouble() < 0.50)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini TwinBear dodges the attack!*");
                // Logic to dodge the attack can be added here
            }

            Random rand = new Random();
            m_NextDodgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20)); // Randomize cooldown
        }

        private void Earthquake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini TwinBear stomps the ground, causing an earthquake!*");
            Effects.PlaySound(Location, Map, 0x54D);
            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    m.SendMessage("You are shaken by the earthquake!");
                }
            }

            Random rand = new Random();
            m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(120, 180)); // Randomize cooldown
        }

        private void EnterFrenzyMode()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini TwinBear goes into a frenzied rage!*");
            m_IsInFrenzy = true;
            SetDamage(15, 25); // Increase damage in frenzy mode
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increase skill in frenzy mode
            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(ExitFrenzyMode));
        }

        private void ExitFrenzyMode()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini TwinBear calms down.*");
            m_IsInFrenzy = false;
            SetDamage(12, 18); // Reset damage
            SetSkill(SkillName.Tactics, 90.0, 110.0); // Reset skill
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

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gemini TwinBear swipes with incredible force!*");
                defender.PlaySound(0x208);
                int damage = Utility.RandomMinMax(7, 14);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
