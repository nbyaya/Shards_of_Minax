using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MarkOfTheBoneboundQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mark of the Bonebound"; } }

        public override object Description
        {
            get
            {
                return
                    "Captain Eliza 'Hookhand' Raven eyes the horizon from the battered dock of Pirate Isle.\n\n" +
                    "Her gaze sharpens as you approach, her right hand clutching the hilt of a weathered cutlass, the left replaced by a gleaming silver hook.\n\n" +
                    "“You’ve seen 'em, haven’t you? The skeletal snipers pickin’ off our watchmen? **Bonebound Marksman**, they call it—a ghost with a crossbow, hauntin’ the towers ever since that cursed raid.”\n\n" +
                    "“It took me crew. Slayed ‘em under moonlight, left nothin’ but bones and broken dreams. But it missed me—by fate or folly, I lived. And I’ve waited long for someone brave—or foolish—enough to hunt it down.”\n\n" +
                    "“**Go to the Exodus Dungeon**. Find the Bonebound Marksman and send it back to whatever pit it came from. Bring peace to me crew’s memory... and I’ll part with a blade dear to me—**the Frostflame Katana**.”\n\n" +
                    "“Don’t fail me, sailor. Their souls don’t rest easy.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then stay out of me sight. I’ve no use for cowards who leave ghosts unavenged.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathin’, are ya? But the Marksman ain’t dead yet, is it? Go back—before it claims more of ours.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done, then? Aye, I can feel it. The night’s quieter already. You’ve done what none else dared.\n\n" +
                       "**Take this—Frostflame Katana.** It’s yours now. May it cut true for you, as it did for me." +
                       "\n\nAnd me thanks… from me and from the dead.";
            }
        }

        public MarkOfTheBoneboundQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BoneboundMarksman), "Bonebound Marksman", 1));
            AddReward(new BaseReward(typeof(FrostflameKatana), 1, "Frostflame Katana"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mark of the Bonebound'!");
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

    public class CaptainElizaRaven : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MarkOfTheBoneboundQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        [Constructable]
        public CaptainElizaRaven()
            : base("the Retired Privateer", "Captain Eliza 'Hookhand' Raven")
        {
        }

        public CaptainElizaRaven(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 40);

            Female = true;
            Body = 0x191; // Female body
            Race = Race.Human;

            Hue = 2010; // Sun-kissed tan
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Storm-grey
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2101, Name = "Stormworn Shirt" }); // Dark blue-gray
            AddItem(new LeatherLegs() { Hue = 2420, Name = "Sea Raider’s Trousers" }); // Seaweed green
            AddItem(new HalfApron() { Hue = 2405, Name = "Captain’s Sash" }); // Faded crimson
            AddItem(new FeatheredHat() { Hue = 1150, Name = "Raven’s Crest" }); // Midnight black
            AddItem(new ThighBoots() { Hue = 2309, Name = "Deckwalker’s Boots" }); // Weathered brown

            AddItem(new Cutlass() { Hue = 1154, Name = "Tidecleaver" }); // Ice-blue blade


            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Salt-Stained Pack";
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
