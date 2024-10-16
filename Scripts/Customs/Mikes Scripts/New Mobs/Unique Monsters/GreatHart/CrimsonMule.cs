using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crimson mule corpse")]
    public class CrimsonMule : BaseCreature
    {
        private DateTime m_NextBloodFrenzy;
        private DateTime m_NextCrimsonCharge;
        private DateTime m_NextHoofStomp;
        private DateTime m_NextSummonMinions;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public CrimsonMule()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a crimson mule";
            Body = 0xEA; // GreatHart body
            Hue = 1989; // Blood-red hue for crimson appearance

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

        public CrimsonMule(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextBloodFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCrimsonCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHoofStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBloodFrenzy && Hits < HitsMax / 2)
                {
                    BloodFrenzy();
                }

                if (DateTime.UtcNow >= m_NextCrimsonCharge && Utility.RandomDouble() < 0.2)
                {
                    CrimsonCharge();
                }

                if (DateTime.UtcNow >= m_NextHoofStomp && Utility.RandomDouble() < 0.15)
                {
                    HoofStomp();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions && Utility.RandomDouble() < 0.1)
                {
                    SummonMinions();
                }
            }
        }

        private void BloodFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crimson Mule goes into a blood frenzy!*");
            FixedEffect(0x376A, 10, 16);

            SetDamage(12, 18); // Increase damage

            m_NextBloodFrenzy = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for frenzy
        }

        private void CrimsonCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crimson Mule's charge leaves you bleeding!*");
            FixedEffect(0x373A, 10, 16);

            if (Combatant != null)
            {
                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
                Mobile mobile = Combatant as Mobile;

                if (mobile != null)
                {
                    mobile.SendMessage("You are bleeding from the charge!");
                }

                // Apply bleed effect
                Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(delegate()
                {
                    if (Combatant != null && !Combatant.Deleted)
                    {
                        if (mobile != null)
                        {
                            mobile.SendMessage("You continue to bleed from the Crimson Mule's charge!");
                        }
                        AOS.Damage(Combatant, this, Utility.RandomMinMax(5, 15), 0, 0, 0, 0, 0);
                    }
                }));
            }

            m_NextCrimsonCharge = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for charge
        }

        private void HoofStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crimson Mule stomps its hooves violently!*");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Crimson Mule's stomp knocks you off balance!");
                    m.PlaySound(0x5C);
                    m.Freeze(TimeSpan.FromSeconds(2)); // Stun effect
                }
            }

            m_NextHoofStomp = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for stomp
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crimson Mule summons its minions!*");
            FixedEffect(0x373A, 10, 16);

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                if (Map.CanSpawnMobile(loc))
                {
                    CrimsonMinion minion = new CrimsonMinion();
                    minion.MoveToWorld(loc, Map);
                    minion.Combatant = this;
                }
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for summoning minions
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
            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }

    public class CrimsonMinion : BaseCreature
    {
        [Constructable]
        public CrimsonMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a crimson minion";
            Body = 0xEA; // Same body as Crimson Mule
            Hue = 1153; // Dark red hue

            SetStr(40, 60);
            SetDex(40, 60);
            SetInt(20, 40);

            SetHits(30, 50);
            SetMana(0);

            SetDamage(6, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Cold, 10, 15);

            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);

            Fame = 200;
            Karma = 0;

            VirtualArmor = 20;
        }

        public CrimsonMinion(Serial serial)
            : base(serial)
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
