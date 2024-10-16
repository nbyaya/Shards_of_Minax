using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a ghost warrior")]
    public class GhostWarrior : BaseCreature
    {
        private TimeSpan m_PhaseDelay = TimeSpan.FromSeconds(15.0); // time between phases
        public DateTime m_NextPhaseTime;

        [Constructable]
        public GhostWarrior() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x190;
            Hue = 0x4001; // Ghostly hue
            Name = "Ghost Warrior";

            SetStr(600, 800);
            SetDex(200, 300);
            SetInt(100, 150);

            SetHits(500, 700);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Anatomy, 50.0, 70.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 60;

            m_NextPhaseTime = DateTime.Now + m_PhaseDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return false; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextPhaseTime)
            {
                if (Utility.RandomBool())
                {
                    this.Say("You cannot touch me!");
                    this.Hidden = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(Unhide));
                }

                m_NextPhaseTime = DateTime.Now + m_PhaseDelay;
            }

            base.OnThink();
        }

        private void Unhide()
        {
            this.Hidden = false;
            this.Say("I have returned!");
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);

            this.Say("My treasures... they remain with me...");
        }

        public GhostWarrior(Serial serial) : base(serial)
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
