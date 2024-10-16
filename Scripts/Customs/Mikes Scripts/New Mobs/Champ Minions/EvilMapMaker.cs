using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a map maker")]
    public class EvilMapMaker : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between map maker speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public EvilMapMaker() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Map Maker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Map Maker";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item sandals = new Sandals(Utility.RandomNeutralHue());
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(sandals);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(200, 300);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(150, 200);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 50.1, 75.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            Fame = 4500;
            Karma = 4500;

            VirtualArmor = 38;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return true; } }
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
                        case 0: this.Say(true, "I see everything from above!"); break;
                        case 1: this.Say(true, "No trap can hide from my sight!"); break;
                        case 2: this.Say(true, "Enemies revealed, your doom is near!"); break;
                        case 3: this.Say(true, "My maps show all secrets!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public void RevealHidden()
        {
            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m.Hidden && this.CanBeHarmful(m))
                {
                    m.RevealingAction();
                    m.SendMessage("You have been revealed by the Map Maker!");
                }
            }

            foreach (Item item in this.GetItemsInRange(10))
            {
                if (item is TrapableContainer || item is BaseTrap)
                {
                    item.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "A trap is revealed!");
                }
            }
        }

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();

            if (this.Combatant != null)
            {
                this.RevealHidden();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My maps... will reveal... the truth..."); break;
                case 1: this.Say(true, "The secrets... are now yours..."); break;
            }

            PackItem(new BlankScroll(Utility.RandomMinMax(10, 20)));
        }

        public EvilMapMaker(Serial serial) : base(serial)
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
