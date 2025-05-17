using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EmeraldEclipseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Emerald Eclipse"; } }

        public override object Description
        {
            get
            {
                return
                    "Vesa Lightkeeper adjusts the brass knobs on an ornate lantern, its flame flickering green.\n\n" +
                    "“Have you ever seen light *flee* from a creature?” she asks, voice heavy with unease.\n\n" +
                    "“The tunnels of Drakkon used to glow by our hand, lanterns burning bright with blessed oil. But now... now they dim whenever **that beast** stirs. An **EmeraldDragon**, drawn to light like a moth, yet it seeks not warmth—but silence.”\n\n" +
                    "“My oil alone holds back its gaze. But it’s not enough. I need someone bold, someone willing to walk into its lair and **extinguish the beast** before it plunges East Montor into darkness.”\n\n" +
                    "“Slay the EmeraldDragon. Return with proof, and our lights will shine again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then walk carefully, for shadows grow bolder with each lantern lost.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The EmeraldDragon still hunts? I feel it even now, dimming our glow.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have returned... and the light holds! The dragon's glare is gone.\n\n" +
                       "Take this **TangDynastyChest**—crafted in an age when light ruled over darkness. May it guard your treasures as surely as you’ve guarded ours.";
            }
        }

        public EmeraldEclipseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EmeraldDragon), "EmeraldDragon", 1));
            AddReward(new BaseReward(typeof(TangDynastyChest), 1, "TangDynastyChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Emerald Eclipse'!");
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

    public class VesaLightkeeper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EmeraldEclipseQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner()); // Closest to lantern/oil supplies
        }

        [Constructable]
        public VesaLightkeeper()
            : base("the Lantern Master", "Vesa Lightkeeper")
        {
        }

        public VesaLightkeeper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 50);

            Female = true;
            Body = 0x191; // Female body type
            Race = Race.Human;

            Hue = 1023; // Pale, almost lantern-glow skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1260; // Soft emerald hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1417, Name = "Lantern-Keeper’s Robe" }); // Deep green robe
            AddItem(new BodySash() { Hue = 1165, Name = "Sash of the Watchfire" }); // Light gold
            AddItem(new Sandals() { Hue = 1175, Name = "Glowfoot Sandals" }); // Subtle green shimmer
            AddItem(new Lantern() { Hue = 1260, Name = "Emerald Oil Lantern" }); // She always carries her light
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Hat of the Everlight" }); // Light gold accent
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
