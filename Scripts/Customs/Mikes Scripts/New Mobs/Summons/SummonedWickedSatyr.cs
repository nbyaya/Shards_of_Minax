using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a wicked satyr corpse")]
    public class SummonedWickedSatyr : BaseCreature
    {
        private DateTime m_NextDreadfulDirge;
        private DateTime m_NextCurseOfDiscord;
        private DateTime m_NextNightmareSong;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SummonedWickedSatyr()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wicked satyr";
            Body = 271; // Satyr body
            Hue = 2296; // Unique hue for the Wicked Satyr
			this.BaseSoundID = 0x586;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Set to false initially
        }

        public SummonedWickedSatyr(Serial serial)
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
                    m_NextDreadfulDirge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCurseOfDiscord = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextNightmareSong = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextDreadfulDirge)
                {
                    DreadfulDirge();
                }

                if (DateTime.UtcNow >= m_NextCurseOfDiscord)
                {
                    CurseOfDiscord();
                }

                if (DateTime.UtcNow >= m_NextNightmareSong)
                {
                    NightmareSong();
                }
            }
        }

        private void DreadfulDirge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a dreadful dirge *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You feel a chilling fear as the Wicked Satyr plays its haunting melody!");
                    m.SendMessage("Your morale is reduced!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            m_NextDreadfulDirge = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Adjust cooldown as needed
        }

        private void CurseOfDiscord()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Curses with discord *");
                target.SendMessage("You feel a deep sense of discord and weakness!");

                target.PlaySound(0x1F1);
                target.FixedEffect(0x376A, 10, 16);

                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (target != null && target.Alive)
                    {
                        // Apply curse effect, increase damage taken
                        target.SendMessage("You are cursed, taking increased damage!");
                        // Adjust damage taken (e.g., increase damage by 20%)
                    }
                });

                m_NextCurseOfDiscord = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Adjust cooldown as needed
            }
        }

        private void NightmareSong()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sings a nightmarish song *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You fall into a nightmarish slumber!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            m_NextNightmareSong = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Adjust cooldown as needed
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
