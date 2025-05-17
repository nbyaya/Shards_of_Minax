using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BreakTheWatchQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Break the Watch"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself in the quiet sanctuary of Yew Chapel, where the flicker of candlelight dances on ancient stone.\n\n" +
                    "*Maelis Shadeveil*, a priestess cloaked in dusk-hued robes, approaches, her eyes marked by both wisdom and dread.\n\n" +
                    "“There is a shadow festering in the old watchtower,” she whispers. “A **DeathlyWatchBeetle** has taken roost—its very presence a blight upon this hallowed ground.”\n\n" +
                    "“Once, I banished carrion swarms from cursed lands, but this... this is something deeper. The beetle feeds on the dead, corrupts the living. Its whispers claw at the minds of my flock.”\n\n" +
                    "“You must ascend the watchtower. **Slay the DeathlyWatchBeetle**. Let the chapel breathe again.”\n\n" +
                    "“Return to me when its chitinous form is no more, and take with you the *SilentVowOfTheWatcher*—a relic forged to guard against necrotic winds.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then tread carefully. The shadow within the tower deepens with every breath, and soon it may reach even here.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beetle still lives? The tower’s bells have not rung true since it arrived. We are silenced, bound by its dread.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have done a sacred thing. The watchtower stands clear once more, its stones no longer weeping darkness.\n\n" +
                       "Take this, *SilentVowOfTheWatcher*, and know that you have restored balance where rot had taken hold.";
            }
        }

        public BreakTheWatchQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DeathlyWatchBeetle), "DeathlyWatchBeetle", 1));
            AddReward(new BaseReward(typeof(SilentVowOfTheWatcher), 1, "SilentVowOfTheWatcher"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Break the Watch'!");
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

    public class MaelisShadeveil : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BreakTheWatchQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHolyMage());
        }

        [Constructable]
        public MaelisShadeveil()
            : base("the Shadowed Priestess", "Maelis Shadeveil")
        {
        }

        public MaelisShadeveil(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 65, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2048; // Long hair
            HairHue = 1150; // Midnight black
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1175, Name = "Veil of Twilight Grace" }); // Dark violet
            AddItem(new Sandals() { Hue = 1153, Name = "Steps of Silence" }); // Cold grey-blue
            AddItem(new Cloak() { Hue = 1150, Name = "Shadowmantle" }); // Midnight black cloak
            AddItem(new BodySash() { Hue = 1170, Name = "Sash of Purity" }); // Deep indigo
            AddItem(new WizardsHat() { Hue = 1175, Name = "Cowl of the Watcher" }); // Dark violet matching robe

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Sanctified Satchel";
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
