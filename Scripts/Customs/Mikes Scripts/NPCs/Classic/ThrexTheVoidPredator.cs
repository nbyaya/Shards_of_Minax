using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class ThrexTheVoidPredator : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ThrexTheVoidPredator() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Threx the Void Predator";
        Body = 0x190; // Human male body

        // Stats
        SetStr(150);
        SetDex(110);
        SetInt(40);
        SetHits(110);

        // Appearance
        AddItem(new BoneChest() { Hue = 1182 });
        AddItem(new BoneGloves() { Hue = 1182 });
        AddItem(new BoneHelm() { Hue = 1181 });
        AddItem(new VikingSword() { Name = "Threx's Ripper" });

        Hue = 1180;
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Threx the Void Predator! My heart beats in rhythm with the cosmos, and my love for void meat surpasses even that.");

        greeting.AddOption("What is void meat?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateVoidMeatModule())); });

        greeting.AddOption("Tell me about your cosmic journey.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJourneyModule())); });

        greeting.AddOption("What do you think of this realm?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateRealmModule())); });

        greeting.AddOption("Do you have any stories to share?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateStoriesModule())); });

        return greeting;
    }

    private DialogueModule CreateVoidMeatModule()
    {
        DialogueModule voidMeatModule = new DialogueModule("Void meat is a delicacy of the cosmic expanse, rich in flavor and steeped in the essence of existence itself.");

        voidMeatModule.AddOption("How do you prepare void meat?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreatePreparationModule())); });

        voidMeatModule.AddOption("What does it taste like?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTasteModule())); });

        voidMeatModule.AddOption("Where can I find it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFindingModule())); });

        voidMeatModule.AddOption("Why do you love it so much?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateLoveModule())); });

        return voidMeatModule;
    }

    private DialogueModule CreatePreparationModule()
    {
        DialogueModule prepModule = new DialogueModule("To prepare void meat, one must first harness the energies of the cosmos. A fire fueled by star essence is ideal.");

        prepModule.AddOption("What ingredients are needed?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateIngredientsModule())); });

        prepModule.AddOption("Is it difficult to cook?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDifficultyModule())); });

        prepModule.AddOption("Can you teach me the recipe?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateRecipeModule())); });

        return prepModule;
    }

    private DialogueModule CreateIngredientsModule()
    {
        return new DialogueModule("The essential ingredients include rare spices from the Nebula Flora, starfire oil, and of course, the prime cut of void meat itself.");
    }

    private DialogueModule CreateDifficultyModule()
    {
        return new DialogueModule("Cooking void meat is not for the faint of heart; it requires precision and an understanding of cosmic energies. One misstep could lead to... explosive results.");
    }

    private DialogueModule CreateRecipeModule()
    {
        DialogueModule recipeModule = new DialogueModule("First, marinate the void meat in starfire oil for an hour. Then, grill it over a star essence fire for a duration corresponding to the phases of the moon.");

        recipeModule.AddOption("What if I mess up?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMessUpModule())); });

        recipeModule.AddOption("Can I make variations?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateVariationsModule())); });

        return recipeModule;
    }

    private DialogueModule CreateMessUpModule()
    {
        return new DialogueModule("If you mess up, don't despair! The void has ways of forgiving, but be cautious—unintended outcomes can lead to bizarre results.");
    }

    private DialogueModule CreateVariationsModule()
    {
        return new DialogueModule("Ah, variations! You can mix in some spices from the realms of light or even pair it with celestial fruits for an unforgettable meal.");
    }

    private DialogueModule CreateTasteModule()
    {
        return new DialogueModule("The taste of void meat is a fusion of flavors from across dimensions; it's savory, with hints of ethereal sweetness that dance upon the tongue.");
    }

    private DialogueModule CreateFindingModule()
    {
        DialogueModule findingModule = new DialogueModule("To find void meat, one must traverse the dimensional rifts. Seek the hidden markets of the intergalactic travelers or negotiate with cosmic entities.");

        findingModule.AddOption("Are there dangers involved?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDangersModule())); });

        return findingModule;
    }

    private DialogueModule CreateDangersModule()
    {
        return new DialogueModule("Indeed! The dimensional rifts are fraught with peril, from rogue time anomalies to cosmic beasts. Only the brave should venture forth.");
    }

    private DialogueModule CreateLoveModule()
    {
        DialogueModule loveModule = new DialogueModule("My love for void meat is rooted in its connection to the cosmos. It embodies the essence of existence and fuels my very being.");

        loveModule.AddOption("Have you shared this love with others?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSharingModule())); });

        return loveModule;
    }

    private DialogueModule CreateSharingModule()
    {
        return new DialogueModule("Indeed! I often share void meat with fellow cosmic wanderers, hosting feasts where we exchange tales of the void while savoring its delights.");
    }

    private DialogueModule CreateJourneyModule()
    {
        return new DialogueModule("My journey through the cosmos has been filled with encounters, lessons, and cosmic marvels. Each moment is a fragment of the grand tapestry of existence.");

        // Additional nested options can be added here
    }

    private DialogueModule CreateRealmModule()
    {
        return new DialogueModule("This realm is but a transient dream, a fleeting shadow of the grand cosmos. I observe its wonders with both curiosity and a hint of melancholy.");
    }

    private DialogueModule CreateStoriesModule()
    {
        DialogueModule storiesModule = new DialogueModule("Ah, stories! Each star I visit has a tale to tell, weaving the fabric of reality with the narratives of existence.");

        storiesModule.AddOption("Tell me a tale!",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTaleModule())); });

        return storiesModule;
    }

    private DialogueModule CreateTaleModule()
    {
        return new DialogueModule("In the early days of the cosmos, when stars were young, a great entity emerged from the void, seeking to bring balance to the raging energies. This entity's legacy is still felt across the dimensions.");
    }

    public ThrexTheVoidPredator(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
