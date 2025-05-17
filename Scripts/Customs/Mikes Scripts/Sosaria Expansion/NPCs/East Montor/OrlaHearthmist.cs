using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BoarsBaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Boar’s Bane"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Orla Hearthmist*, East Montor’s seasoned innkeeper, apron stained with herbs and flour.\n\n" +
                    "Her eyes flicker with both warmth and weary frustration.\n\n" +
                    "“Have you seen the gardens? Trampled. Crushed under tusk and hoof. The *DrakonBoar*—spawn of Drakkon’s cursed brood—comes at night.”\n\n" +
                    "“I’ve tended those beds since I was a lass. Grew herbs that heal, herbs that warm the spirit. But now? All ruined.”\n\n" +
                    "“Guests report *snorts in the dark*, louder than any beast I’ve known. They fear to stay. My hearth grows cold without them.”\n\n" +
                    "“Will you help me? **Slay the DrakonBoar** and let the gardens grow free again.”\n\n" +
                    "She presses a charm into your hand—an *Alchemical Talisman* she swears will bring strength in battle.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then my gardens shall wither, and this hearth shall grow colder still.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The boar still roams? I hear its foul snorts in my dreams. Please, don't leave my gardens to ruin.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast is slain? Truly? Bless you! My herbs shall grow again, and the inn shall bloom with life.\n\n" +
                       "Take this: the *AlchemyTalisman*. May it bring you the strength you brought me, friend.";
            }
        }

        public BoarsBaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonBoar), "DrakonBoar", 1));
            AddReward(new BaseReward(typeof(AlchemyTalisman), 1, "AlchemyTalisman"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Boar’s Bane'!");
            Owner.PlaySound(CompleteSound);
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

    public class OrlaHearthmist : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BoarsBaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBInnKeeper());
        }

        [Constructable]
        public OrlaHearthmist()
            : base("the Innkeeper", "Orla Hearthmist")
        {
        }

        public OrlaHearthmist(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2048; // Long Hair
            HairHue = 1153; // Warm auburn
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2101, Name = "Mistweave Gown" }); // Soft grey-blue
            AddItem(new FullApron() { Hue = 2405, Name = "Herb-Stained Apron" }); // Earthy green
            AddItem(new Sandals() { Hue = 2413, Name = "Hearthkeeper’s Sandals" }); // Warm brown
            AddItem(new Bonnet() { Hue = 1150, Name = "Garden Bonnet" }); // Light blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Innkeeper’s Pack";
            AddItem(backpack);
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
