using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BridgeOfBonesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bridge of Bones"; } }

        public override object Description
        {
            get
            {
                return
                    "Morwen Tidecaller, a fisherwoman whose nets pull more than fish from the river, stands by Yew’s quiet waters.\n\n" +
                    "Her weathered hands clutch fragments of bone, wet and cold, dredged from the depths beneath the bridge.\n\n" +
                    "“The **PutridBoneGargoyle** haunts the old crossing,” she says, her voice low. “It shattered the wards that kept us safe. Each day, I cross that bridge to cast my nets, and each day I feel the pull of something rotten below.”\n\n" +
                    "“The bones you see? My nets found them, tangled in weeds. They whisper now. I need that bridge safe. The waters are calling, but I won’t answer until **you slay the beast**.”\n\n" +
                    "**Defeat the PutridBoneGargoyle**, restore the ward, and let the river run free once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I’ll find another way across… or not at all. But the river remembers, and the bones will not wait forever.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it roams? I hear it now, in the currents. The bridge groans with each step I take.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The bridge… it’s still, and the bones no longer call. Thank you, wanderer. \n\n" +
                       "Take **Deepcaller**—I carved it from driftwood and iron. It listens to the depths. May it guide you as it has guided me.";
            }
        }

        public BridgeOfBonesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PutridBoneGargoyle), "PutridBoneGargoyle", 1));
            AddReward(new BaseReward(typeof(Deepcaller), 1, "Deepcaller"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bridge of Bones'!");
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

	public class MorwenTidecaller : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(BridgeOfBonesQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBFisherman());
		}

		[Constructable]
		public MorwenTidecaller()
			: base("the River’s Voice", "Morwen Tidecaller")
		{
		}

		public MorwenTidecaller(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(75, 80, 30);

			Female = true;
			Body = 0x191; // Female
			Race = Race.Human;

			Hue = 1150; // Pale river-toned skin
			HairItemID = 0x203C; // Long hair
			HairHue = 1109; // Dark, stormy gray
		}

		public override void InitOutfit()
		{
			AddItem(new Skirt() { Hue = 2210, Name = "Riverwoven Skirt" }); // Deep green-blue
			AddItem(new FancyShirt() { Hue = 2218, Name = "Tidecaller’s Blouse" }); // Ocean teal
			AddItem(new Cloak() { Hue = 2403, Name = "Netwoven Cloak" }); // Washed-out gray, looks like fishing nets
			AddItem(new Sandals() { Hue = 1816, Name = "Silted Sandals" }); // Mud-brown
			AddItem(new Bandana() { Hue = 1153, Name = "Stormwrap" }); // Sea blue

			AddItem(new FishermansTrident() { Hue = 1175, Name = "Bonehook Trident" }); // Rusted iron tone

			Backpack backpack = new Backpack();
			backpack.Hue = 1160;
			backpack.Name = "River Satchel";
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
