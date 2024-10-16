using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a shadow golem corpse")]
    public class ShadowGolem : BaseCreature
    {
        private DateTime m_NextShadowStrike;
        private DateTime m_NextDarkVeil;
        private DateTime m_NextPhaseShift;
        private DateTime m_NextShadowClone;
        private DateTime m_NextDarkSurge;
        private DateTime m_NextShadowEnsnare;
        private bool m_AbilitiesInitialized; // Added this field to track if abilities have been initialized
        private bool m_IsEnraged; // Added this field to track enraged state

        [Constructable]
        public ShadowGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a shadow golem";
            Body = 752; // Golem body
            Hue = 1923; // Dark hue
			BaseSoundID = 357;

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

            m_AbilitiesInitialized = false; // Initialize the flag to false
        }

        public ShadowGolem(Serial serial) : base(serial)
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
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowEnsnare = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextDarkVeil)
                {
                    DarkVeil();
                }

                if (DateTime.UtcNow >= m_NextPhaseShift)
                {
                    PhaseShift();
                }

                if (DateTime.UtcNow >= m_NextShadowClone)
                {
                    ShadowClone();
                }

                if (DateTime.UtcNow >= m_NextDarkSurge)
                {
                    DarkSurge();
                }

                if (DateTime.UtcNow >= m_NextShadowEnsnare)
                {
                    ShadowEnsnare();
                }

                if (Hits < HitsMax * 0.25 && !m_IsEnraged) // Use m_IsEnraged here
                {
                    Enrage();
                }
            }
        }

        private void ShadowStrike()
        {
            if (Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem vanishes into the shadows and strikes with great force! *");

            if (Combatant is Mobile mob)
            {
                mob.SendMessage("The Shadow Golem strikes you from the shadows!");
            }

            int damage = Utility.RandomMinMax(30, 50);
            AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

            m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void DarkVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem envelops the area in a dark veil! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Player)
                {
                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are engulfed in a suffocating darkness!");
                    }
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
            {
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m != Combatant)
                    {
                        int damage = Utility.RandomMinMax(10, 20);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }
            }));

            m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem phases out of reality, becoming incorporeal! *");

            Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(delegate
            {
                if (!Deleted)
                {
                    MoveToWorld(Location, Map);
                }
            }));

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ShadowClone()
        {
            if (Deleted || Combatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem summons shadowy clones to attack! *");

            for (int i = 0; i < 2; i++)
            {
                ShadowGolemClone clone = new ShadowGolemClone();
                clone.Team = Team;
                clone.MoveToWorld(Location, Map);
                clone.Combatant = Combatant;
            }

            m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DarkSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem unleashes a surge of dark energy! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are struck by a powerful surge of dark energy!");
                    }
                }
            }

            m_NextDarkSurge = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ShadowEnsnare()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem ensnares you in shadowy tendrils! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this)
                {
                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are ensnared by shadowy tendrils and feel your strength wane!");
                    }

                    // Replace Slow with a suitable effect
                    // Assuming you have an appropriate method to slow down the Mobile
                    // m.Slow(TimeSpan.FromSeconds(10)); // Remove or replace this line
                    m.Damage(10);
                }
            }

            m_NextShadowEnsnare = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void Enrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Golem roars with fury and becomes more aggressive! *");
            SetDamage(30, 40); // Increase damage
            SetResistance(ResistanceType.Physical, 70); // Increase physical resistance

            // Set enraged flag
            m_IsEnraged = true;

            // Reduce cooldowns for other abilities
            m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            m_NextDarkSurge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextShadowEnsnare = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
        }
    }

    public class ShadowGolemClone : BaseCreature
    {
        [Constructable]
        public ShadowGolemClone()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "shadow clone";
            Body = 752; // Golem body
            Hue = 1150; // Dark hue

            SetStr(150);
            SetDex(50);
            SetInt(50);

            SetHits(200);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20);
            SetResistance(ResistanceType.Fire, 10);
            SetResistance(ResistanceType.Cold, 10);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 10);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 40.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;

            Tamable = false;
        }

        public ShadowGolemClone(Serial serial) : base(serial)
        {
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
        }
    }
}
