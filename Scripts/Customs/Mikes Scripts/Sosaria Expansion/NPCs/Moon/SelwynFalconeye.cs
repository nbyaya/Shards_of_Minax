using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RacingUndeadQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Racing Undead"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Selwyn Falconeye*, lead scout of the Moon Pyramid patrols.\n\n" +
                    "“It’s no ordinary beast—this *Mummified Ostard*. Fast as desert wind, with a beak like a damned lance. It tramples our caravans just as the sun crests the dunes. I’m sick of chasing its shadow.”\n\n" +
                    "**Hunt down the Mummified Ostard** before it claims more lives at dawn.";
            }
        }

        public override object Refuse { get { return "Then pray your path never crosses that monster at sunrise. Few live to tell of it."; } }

        public override object Uncomplete { get { return "It still roams? Every second it breathes, more blood spills on the sands."; } }

        public override object Complete { get { return "You’ve done it? The beast is no more... You've outpaced death itself. Take this, may it serve you as keenly as your blade served us."; } }

        public RacingUndeadQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MummifiedOstard), "Mummified Ostard", 1));
            AddReward(new BaseReward(typeof(Shardgrin), 1, "Shardgrin"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Racing Undead'!");
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

    public class SelwynFalconeye : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RacingUndeadQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHolyMage()); 
        }

        [Constructable]
        public SelwynFalconeye()
            : base("the Falcon Scout", "Selwyn Falconeye")
        {
        }

        public SelwynFalconeye(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 90, 50);

            Female = false;
            Body = 0x190; // Male Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue(); 
            HairItemID = Race.RandomHair(this);
            HairHue = 1109; // Dusty Sand - Pale blonde
            FacialHairItemID = 0x203F; // Full beard
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            // Unique Outfit for Desert Scout
            AddItem(new LeatherNinjaHood() { Hue = 1815, Name = "Falcon Hood" }); // Sandy beige
            AddItem(new LeatherDo() { Hue = 1816, Name = "Windswept Leather Tunic" }); // Desert tan
            AddItem(new LeatherHaidate() { Hue = 1816, Name = "Falcon Guard Leggings" }); // Matched tan
            AddItem(new LeatherGloves() { Hue = 1109, Name = "Scout's Grip" }); // Light leather
            AddItem(new NinjaTabi() { Hue = 1815, Name = "Silent Step Boots" }); // Sand-hued footwear
            AddItem(new Cloak() { Hue = 2301, Name = "Cloak of the Morning Vigil" }); // Pale sunrise orange
            AddItem(new CompositeBow() { Hue = 2406, Name = "Ostardbane" }); // Dark wood, used in tracking
            Backpack backpack = new Backpack();
            backpack.Hue = 2434;
            backpack.Name = "Falcon Pack";
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
