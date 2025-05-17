using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AzureMenaceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Azure Menace"; } }

        public override object Description
        {
            get
            {
                return
                    "Corin Oakshield, Guard Captain of East Montor, stands firm upon the battlements, his gaze scanning the distant peaks of the Caves of Drakkon.\n\n" +
                    "His armor is heavy, but it’s the **talisman around his neck**—a weathered charm of woven silver and frost quartz—that seems to carry the most weight.\n\n" +
                    "\"The blue beast comes at dawn, or when the skies are clearest,\" Corin says, his voice sharp like the mountain winds. \n\n" +
                    "\"It breathes ice and terror—**freezing our watchtowers**, cracking the stone, numbing the resolve of my men. If it strikes true again, our walls will fall silent in the frost.\"\n\n" +
                    "\"My grandmother faced dragons in her youth, and gave me this charm for strength. But charms won’t kill it. **Only steel can**.\"\n\n" +
                    "**Slay the BlueDragon** that haunts our skies. Return with proof, and I'll reward you with something worthy of a warrior of the old blood.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray for warmth, for we’ll need fires brighter than hope to survive the dragon’s breath.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? Then every moment you tarry, another brick of our walls shatters under frost. Go. Find its heart—and still it.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain the Azure Menace... and East Montor stands because of you.\n\n" +
                       "**Take this TribalHelm**—once worn by my ancestors in the frozen marches. Let it shield you as you’ve shielded us.";
            }
        }

        public AzureMenaceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlueDragon), "BlueDragon", 1));
            AddReward(new BaseReward(typeof(TribalHelm), 1, "TribalHelm"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Azure Menace'!");
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

    public class CorinOakshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AzureMenaceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer()); // As a Guard Captain, Plate Armor suits his role.
        }

        [Constructable]
        public CorinOakshield()
            : base("the Guard Captain", "Corin Oakshield")
        {
        }

        public CorinOakshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Silver-gray
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1355, Name = "Frostbound Plate" }); // Pale blue steel
            AddItem(new PlateLegs() { Hue = 1355, Name = "Frostbound Greaves" });
            AddItem(new PlateArms() { Hue = 1355, Name = "Frostbound Vambraces" });
            AddItem(new PlateGloves() { Hue = 1150, Name = "Warder’s Gauntlets" });
            AddItem(new WingedHelm() { Hue = 1355, Name = "Helm of the Watch" });
            AddItem(new Cloak() { Hue = 1109, Name = "Captain’s Cloak of the North" });
            AddItem(new BodySash() { Hue = 2101, Name = "Oakshield Talisman Sash" });

            AddItem(new Broadsword() { Hue = 1153, Name = "Coldsteel Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Guard Captain's Pack";
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
