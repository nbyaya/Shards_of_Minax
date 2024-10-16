using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Vladimir Lenin")]
public class VladimirLenin : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public VladimirLenin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Vladimir Lenin";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(80);
        SetInt(90);
        SetHits(75);

        // Appearance
        AddItem(new LongPants() { Hue = 1910 });
        AddItem(new Tunic() { Hue = 1102 });
        AddItem(new Boots() { Hue = 1102 });
        AddItem(new WarAxe() { Name = "Bolshevik Blade" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("I am Vladimir Lenin, the revolutionary! How may I inspire you today?");

        greeting.AddOption("Tell me about revolutionary theory.",
            player => true,
            player =>
            {
                DialogueModule theoryModule = new DialogueModule("Revolutionary theory is the guiding light for those seeking to change the world. It encompasses various schools of thought. Which aspect interests you?");
                theoryModule.AddOption("What is Marxism?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule marxismModule = new DialogueModule("Marxism is a socioeconomic analysis that uses a materialist interpretation of historical development, often referred to as historical materialism. It posits that the struggle between the ruling class and the working class is the engine of historical change.");
                        marxismModule.AddOption("Tell me about class struggle.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Class struggle is the conflict between different classes within society, particularly the bourgeoisie (capitalist class) and the proletariat (working class). This struggle is a key factor in societal evolution and revolutionary change.")));
                            });
                        marxismModule.AddOption("What about dialectical materialism?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Dialectical materialism combines Hegelian dialectics and materialism. It argues that material conditions shape consciousness and that social change arises from contradictions within society, ultimately leading to revolutionary outcomes.")));
                            });
                        player.SendGump(new DialogueGump(player, marxismModule));
                    });

                theoryModule.AddOption("What is socialism?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule socialismModule = new DialogueModule("Socialism is an economic and political system where the means of production are owned and regulated by the community as a whole. It seeks to establish a more equitable distribution of wealth and to eliminate class distinctions.");
                        socialismModule.AddOption("How does socialism differ from communism?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("While both aim for a classless society, socialism often exists as a transitional phase towards communism. Socialism emphasizes state control of resources, while communism seeks to abolish the state altogether.")));
                            });
                        socialismModule.AddOption("What are the criticisms of socialism?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Critics argue that socialism can lead to inefficiencies and a lack of innovation due to state control. They also express concerns over the potential for authoritarian governance.")));
                            });
                        player.SendGump(new DialogueGump(player, socialismModule));
                    });

                theoryModule.AddOption("What about anarchism?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule anarchismModule = new DialogueModule("Anarchism advocates for a stateless society where individuals freely cooperate without hierarchical structures. Anarchists believe in dismantling oppressive institutions, often opposing both capitalism and state control.");
                        anarchismModule.AddOption("How do anarchists view revolution?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Anarchists see revolution as a means to overthrow oppressive systems, but they emphasize grassroots organization and direct action rather than seizing state power.")));
                            });
                        anarchismModule.AddOption("What are some anarchist theories?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Prominent theories include anarcho-communism, which advocates for communal ownership of resources, and anarcho-syndicalism, which focuses on labor movements and workers' self-management.")));
                            });
                        player.SendGump(new DialogueGump(player, anarchismModule));
                    });

                player.SendGump(new DialogueGump(player, theoryModule));
            });

        greeting.AddOption("What is your vision for the revolution?",
            player => true,
            player =>
            {
                DialogueModule visionModule = new DialogueModule("My vision is a world where the working class rises against oppression and seizes the means of production. Through unity and collective action, we can create a society where resources are shared and all voices are heard.");
                visionModule.AddOption("How can individuals contribute to the revolution?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Every individual can contribute by educating themselves and others about revolutionary theory, participating in grassroots organizing, and advocating for the rights of the working class. Every small action counts!")));
                    });
                visionModule.AddOption("What challenges do we face?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("The primary challenges include misinformation, apathy, and the powerful interests of the ruling class who will do everything to maintain their dominance. Overcoming these obstacles requires dedication and courage.")));
                    });
                player.SendGump(new DialogueGump(player, visionModule));
            });

        greeting.AddOption("What do you think of unity?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Unity is crucial for any revolutionary movement. Only through collective strength can we dismantle the oppressive structures that bind us. Together, we can overcome any obstacle.")));
            });

        return greeting;
    }

    public VladimirLenin(Serial serial) : base(serial) { }

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
