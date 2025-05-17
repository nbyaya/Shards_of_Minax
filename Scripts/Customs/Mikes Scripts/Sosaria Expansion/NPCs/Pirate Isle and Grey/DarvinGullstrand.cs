using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BanditsShadowQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bandit's Shadow"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Darvin Gullstrand*, Fisher Captain of Pirate Isle, a man of tides and tempests.\n\n" +
                    "His weathered coat smells faintly of salt and smoke, eyes narrowed against memories of loss.\n\n" +
                    "“There’s a shadow stalkin’ these shores, stranger—a cursed bandit with claws like daggers. *Shadowclaw,* they call him. Boarded my ship near the misty banks of the old river trail. Took my wares, near gutted my mate.”\n\n" +
                    "“I’ve tracked him to the old ruins near Exodus Dungeon. I’m no warrior—I cast nets, not blades. But I’ll not stand idle while he haunts the river.”\n\n" +
                    "**Hunt down the Shadowclaw Bandit** and make the waters safe again.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then steer clear of the river, friend. For the shadow lingers, and it strikes when you least expect.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still he haunts the misty banks? The merchants grow fearful, and fewer boats sail with each dawn.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain the clawed devil? Ha! Bless the tides, and bless your blade.\n\n" +
                       "Here, take this *Bag of Health*. May it keep you strong in the face of darker tides.";
            }
        }

        public BanditsShadowQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ShadowclawBandit), "Shadowclaw Bandit", 1));
            AddReward(new BaseReward(typeof(BagOfHealth), 1, "Bag of Health"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bandit's Shadow'!");
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

    public class DarvinGullstrand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BanditsShadowQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        [Constructable]
        public DarvinGullstrand()
            : base("the Fisher Captain", "Darvin Gullstrand")
        {
        }

        public DarvinGullstrand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 75, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Short Hair
            HairHue = 1102; // Seafoam-gray
            FacialHairItemID = 0x2041; // Short Beard
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1107, Name = "Salt-Stained Shirt" });
            AddItem(new ShortPants() { Hue = 1108, Name = "Storm-Worn Breeches" });
            AddItem(new Boots() { Hue = 1101, Name = "Tidewalkers" });
            AddItem(new BodySash() { Hue = 1154, Name = "Captain's Sash" });
            AddItem(new TricorneHat() { Hue = 1150, Name = "Windshadow Hat" });
            AddItem(new Cutlass() { Hue = 2406, Name = "Harborfang" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Captain's Sea Bag";
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
