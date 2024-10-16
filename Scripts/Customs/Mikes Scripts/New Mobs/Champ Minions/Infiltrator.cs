using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an infiltrator")]
    public class Infiltrator : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Infiltrator() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // Human body
            Name = NameList.RandomName("male");
            Title = " the Infiltrator";

            Item hoodedShroud = new HoodedShroudOfShadows();
            hoodedShroud.Hue = 1175;
            AddItem(hoodedShroud);

            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            AddItem(boots);

            Item dagger = new Dagger();
            AddItem(dagger);
            dagger.Movable = false;

            SetStr(500, 700);
            SetDex(300, 400);
            SetInt(150, 200);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 50.0, 70.0);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);
            SetSkill(SkillName.Snooping, 100.0, 120.0);
            SetSkill(SkillName.Fencing, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You'll never see me coming!"); break;
                        case 1: this.Say(true, "I've already won."); break;
                        case 2: this.Say(true, "Shadows hide my approach."); break;
                        case 3: this.Say(true, "You can't fight what you can't see!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "They'll never know what hit them..."); break;
                case 1: this.Say(true, "I've failed... for now..."); break;
            }

            PackItem(new Nightshade(Utility.RandomMinMax(10, 20)));
        }

        public Infiltrator(Serial serial) : base(serial)
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
