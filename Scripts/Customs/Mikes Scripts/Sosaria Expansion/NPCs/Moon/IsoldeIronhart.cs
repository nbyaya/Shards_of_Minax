using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MummifiedEliteQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mummified Elite"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Isolde Ironhart*, steadfast defender of Moon’s royal halls.\n\n" +
                    "“A threat stirs beneath the sands—the *Elite Mummy*, clad in ancient armor resistant to steel. My spies speak of a hidden vault under The Pyramid, where this creature guards relics of power.\n\n" +
                    "Steel will fail unless you strike at the runic gaps etched into its defenses.\n\n" +
                    "**Defeat the Elite Mummy** and ensure it never rises again.”";
            }
        }

        public override object Refuse { get { return "I will find another, then. But know this—such evils grow bolder if left unchecked."; } }

        public override object Uncomplete { get { return "It still walks? Then time is running short—strike where the runes are weakest."; } }

        public override object Complete { get { return "Well done. You have safeguarded Moon, and your name will not be forgotten. Take this, as thanks from the palace guard."; } }

        public MummifiedEliteQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EliteMummy), "Elite Mummy", 1));
            AddReward(new BaseReward(typeof(WardenOfTheGrove), 1, "WardenOfTheGrove"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mummified Elite'!");
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

    public class IsoldeIronhart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MummifiedEliteQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this)); 
        }

        [Constructable]
        public IsoldeIronhart()
            : base("the Palace Guard", "Isolde Ironhart")
        {
        }

        public IsoldeIronhart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 50);

            Female = true;
            Body = 0x191; // Female Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue(); // Hair and skin tone
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = 0; // None
        }

        public override void InitOutfit()
        {
            // Unique Outfit for Isolde Ironhart
            AddItem(new PlateChest() { Hue = 2425, Name = "Moonsteel Cuirass" }); // Shimmering pale silver-blue
            AddItem(new PlateLegs() { Hue = 2425, Name = "Moonsteel Greaves" });
            AddItem(new PlateArms() { Hue = 2425, Name = "Moonsteel Pauldrons" });
            AddItem(new PlateGloves() { Hue = 2425, Name = "Moonsteel Gauntlets" });
            AddItem(new PlateHelm() { Hue = 1153, Name = "Helm of Lunar Vigilance" }); // Deep cosmic blue
            AddItem(new Cloak() { Hue = 1150, Name = "Warden’s Cloak" }); // Pale silver
            AddItem(new Boots() { Hue = 1154, Name = "Ironhart’s Tread" }); // Steely grey

            AddItem(new Broadsword() { Hue = 2407, Name = "Defender’s Oath" }); // Glowing steel blade
            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Guard's Pack";
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
