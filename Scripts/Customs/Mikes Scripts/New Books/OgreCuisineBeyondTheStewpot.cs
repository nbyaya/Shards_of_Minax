using System;
using Server;

namespace Server.Items
{
    public class OgreCuisineBeyondTheStewpot : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ogre Cuisine: Beyond the Stewpot", "Chef Grunthar",
            new BookPageInfo
            (
                "As an ogre chef of",
                "renown, Grunthar I be.",
                "This book, a treasure",
                "of ogre gastronomy,",
                "beyond simple stews",
                "and raw meat feasts.",
                "A culinary adventure",
                "for the robust appetite."
            ),
            new BookPageInfo
            (
                "Forget what ye heard",
                "of ogre diets crude,",
                "we've arts in the",
                "kitchen, misunderstood.",
                "From the Mushroom",
                "Swamp, pick fungi",
                "bold, stew with toad's",
                "breath, a dish best served cold."
            ),
            new BookPageInfo
            (
                "Take ye the giant snail,",
                "slow it may be,",
                "its flesh cooked in",
                "embers, a delight for thee.",
                "And the eye of cyclops,",
                "a rare gourmet treat,",
                "in dragon's fire roasted,",
                "an unbeatable feast."
            ),
            new BookPageInfo
            (
                "For a snack, munch on",
                "crispy bat wings,",
                "with a pinch of salt,",
                "and other small things.",
                "Don't ye dare to ignore",
                "the humble rock grub,",
                "its taste quite refined",
                "when dipped in swamp mud."
            ),
            new BookPageInfo
            (
                "Now, ogres love pies,",
                "but not as ye know.",
                "We stuff ours with",
                "insects, a squirming show.",
                "Bake until golden,",
                "the crust nice and thick,",
                "serve with a garnish",
                "of pickled troll kick."
            ),
            new BookPageInfo
            (
                "For a hearty main course,",
                "goblin stew is grand,",
                "throw in some roots",
                "and a handful of sand.",
                "Let it bubble and cook,",
                "in a cauldron o'er flame,",
                "season with anger,",
                "and sorrow, and shame."
            ),
            new BookPageInfo
            (
                "To end, Ogre's Delight,",
                "a dessert to behold,",
                "cave mushrooms glazed",
                "with slime, bold and cold.",
                "Sprinkle with dust",
                "from a vampire's tomb,",
                "this treat, I assure,",
                "makes all ogres swoon."
            ),
            new BookPageInfo
            (
                "This book of recipes,",
                "from my kitchen, I give.",
                "Cook with care, eat with joy,",
                "and as ogres, let's live.",
                "May your pot always bubble,",
                "your meat never lean,",
                "and may your meals cause",
                "the weak-hearted to keen."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Chef Grunthar",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your belly never",
                "know the pang of hunger."
            ),
			new BookPageInfo
            (
                "Should ye be craving",
                "something exotic to try,",
                "seek ye the hydra's heart,",
                "in swamp water fried.",
                "Its many flavors will",
                "dance upon thy tongue,",
                "a symphony of tastes,",
                "unrivaled and unsung."
            ),
            new BookPageInfo
            (
                "Now every ogre chef",
                "with his salt must know,",
                "the art of preparing",
                "a succulent crow.",
                "Pluck the feathers clean,",
                "stuff with spicy leaves,",
                "wrap in basilisk skin,",
                "and bake 'neath hot eaves."
            ),
            new BookPageInfo
            (
                "If yer sweet tooth itches,",
                "and ye yearn for a treat,",
                "mix fire ants with sugar,",
                "for a snack that’s neat.",
                "Their fiery burst within",
                "the sweetness, so profound,",
                "will have ye stomping",
                "with joy upon the ground."
            ),
            new BookPageInfo
            (
                "For a brew that warms",
                "the belly and the soul,",
                "boil the water of a bog",
                "with coal from a troll.",
                "Add the petals of a",
                "dark and withered rose,",
                "drink down the bitter sips,",
                "feel the strength it bestows."
            ),
            new BookPageInfo
            (
                "On days when battle looms",
                "and courage ye need find,",
                "prepare the blood pudding,",
                "with entrails entwined.",
                "Season with the despair",
                "of your conquered foes,",
                "and gulp it down quick",
                "before off to war ye goes."
            ),
            new BookPageInfo
            (
                "When the winter winds howl,",
                "and ye seek to stay warm,",
                "stew elderberry with",
                "wooly mammoth form.",
                "Let it sit o’er the night,",
                "in the glow of the hearth,",
                "come morn, this concoction",
                "will prove its worth."
            ),
            new BookPageInfo
            (
                "And should ye find love,",
                "a rare dish to serve,",
                "brew a potion of passion",
                "with the nerve to unnerve.",
                "Toss in two hearts beating",
                "in synchronized time,",
                "drink with thine ogre lass,",
                "and enjoy love's sublime."
            ),
            new BookPageInfo
            (
                "This tome of ogre cuisine",
                "ends on this page.",
                "Cook with boldness, live large,",
                "let your kitchen be your stage.",
                "Remember, what others find",
                "gruesome or queer,",
                "for us ogres, is delightful,",
                "and brings us cheer."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Chef Grunthar",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Eat well, live strong,",
                "and may your cooking",
                "forever be hearty",
                "and your pot never empty."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OgreCuisineBeyondTheStewpot() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ogre Cuisine: Beyond the Stewpot");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ogre Cuisine: Beyond the Stewpot");
        }

        public OgreCuisineBeyondTheStewpot(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
