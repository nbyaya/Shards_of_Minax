using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Torren, the Haunted Apprentice")]
    public class Torren : BaseCreature
    {
        [Constructable]
        public Torren() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Torren";
            Body = 0x190; // Human male body

            // Basic Stats
            SetStr(90);
            SetDex(80);
            SetInt(85);
            SetHits(100);

            // Appearance and Outfit - a builder’s work-worn attire
            AddItem(new Shirt());
            AddItem(new ShortPants());
            AddItem(new Boots());
            Hue = Utility.RandomSkinHue();

            // Optionally add a builder’s cap or tool belt.
            // AddItem(new BuilderCap() { Hue = 1100 });
        }

        public Torren(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Greetings, traveler! I'm Torren—an eager apprentice builder at Castle British. " +
                "I work tirelessly under Sir Thomund’s guidance to restore ancient battlements, " +
                "and I collaborate with Arlo from Dawn on community projects. Yet, behind these " +
                "practical endeavors lies a secret: I am also haunted by memories of my former life as a soldier. " +
                "The discipline I learned and the battles I fought still echo in my heart. " +
                "What would you like to discuss?"
            );

            // Option 1: Castle Repairs and Mentorship
            greeting.AddOption("Tell me about your work on castle repairs.", 
                player => true, 
                player =>
                {
                    DialogueModule castleRepairModule = new DialogueModule(
                        "Castle repairs are a living art, where every stone speaks of our history. " +
                        "Under Sir Thomund’s precise tutelage, I've learned that each repair is not just a task but a tribute " +
                        "to the past. Our current project involves reinforcing a damaged battlement where the enchanted resin " +
                        "binds stones together. Would you like to learn more about the repairs, our techniques, or the magical resin?"
                    );
                    castleRepairModule.AddOption("Describe the repairs in detail.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule detailModule = new DialogueModule(
                                "We're laboring over an ancient wall segment crumbling under the weight of time. " +
                                "Every block is placed with careful precision. Sir Thomund insists that even a single misplaced stone " +
                                "could unravel centuries of fortification. The magic resin, imbued with the blessings of local woodland spirits, " +
                                "is our secret weapon against decay."
                            );
                            detailModule.AddOption("How does the resin work?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule resinModule = new DialogueModule(
                                        "The resin is said to be harvested from trees that have witnessed the castle's many battles. " +
                                        "It hardens as if challenged by time yet remains flexible enough to absorb nature’s shifting forces. " +
                                        "Some say that the resin carries whispers of old victories and losses, a reminder of what once was."
                                    );
                                    resinModule.AddOption("Remarkable—thanks for sharing.", 
                                        pl3 => true, 
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, resinModule));
                                });
                            detailModule.AddOption("I appreciate the craftsmanship.", 
                                pl2 => true, 
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, detailModule));
                        });
                    castleRepairModule.AddOption("Tell me about your building techniques.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule techniquesModule = new DialogueModule(
                                "We combine age-old masonry with subtle enchantments. Keystone arches ensure stability, " +
                                "and every scaffold is erected with military precision—perhaps a remnant of my past discipline. " +
                                "Sir Thomund often says, 'Build not just with your hands, but with your heart and mind as well.'"
                            );
                            techniquesModule.AddOption("Intriguing philosophy.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            techniquesModule.AddOption("Are there magical methods involved?", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule magicModule = new DialogueModule(
                                        "Indeed. On the toughest projects, we weave in minor enchantments to fortify the structure. " +
                                        "These subtle magics work behind the scenes, ensuring the integrity of our work against nature's wrath."
                                    );
                                    magicModule.AddOption("I see. Thanks for explaining.", 
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, magicModule));
                                });
                            pl.SendGump(new DialogueGump(pl, techniquesModule));
                        });
                    player.SendGump(new DialogueGump(player, castleRepairModule));
                });

            // Option 2: Mentor - Sir Thomund
            greeting.AddOption("Tell me about Sir Thomund, your mentor.", 
                player => true,
                player =>
                {
                    DialogueModule mentorModule = new DialogueModule(
                        "Sir Thomund is a paragon of craftsmanship and discipline. " +
                        "A former warrior himself, his body bears the scars of long-forgotten battles. " +
                        "He demands precision and instills in us the values of honor and meticulous work. " +
                        "Would you like to hear a particular lesson he gave me or the details of his storied past?"
                    );
                    mentorModule.AddOption("Share a memorable lesson from him.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule lessonModule = new DialogueModule(
                                "I recall when a single misaligned stone almost collapsed a critical wall section. " +
                                "Sir Thomund gathered us and, with the calm of a seasoned soldier, demonstrated the exacting art " +
                                "of reinforcements. 'A fortress is built on discipline and honor,' he pronounced. " +
                                "That moment stayed with me, as did his unwavering belief in precision."
                            );
                            lessonModule.AddOption("I understand why he's so revered.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, lessonModule));
                        });
                    mentorModule.AddOption("Tell me more about his past.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule pastModule = new DialogueModule(
                                "Before becoming the master builder of Castle British, Sir Thomund was an indomitable soldier. " +
                                "His experiences on the battlefield taught him the value of every stone, as each could be a shield " +
                                "against the chaos of war. His wisdom reflects the sacrifices he made; indeed, every scar is a tale of valor and loss."
                            );
                            pastModule.AddOption("A formidable warrior-craftsman indeed.", 
                                pl2 => true, 
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, pastModule));
                        });
                    player.SendGump(new DialogueGump(player, mentorModule));
                });

            // Option 3: Collaborations with Arlo of Dawn
            greeting.AddOption("What about your collaborations with Arlo from Dawn?", 
                player => true,
                player =>
                {
                    DialogueModule arloModule = new DialogueModule(
                        "Arlo is not only a visionary architect but also a kindred spirit. " +
                        "We merge traditional techniques with modern designs, like in the community center project " +
                        "that serves as a lifeline for the locals. Would you prefer to hear more about the project details " +
                        "or Arlo’s innovative methods?"
                    );
                    arloModule.AddOption("Tell me more about the community center.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule communityModule = new DialogueModule(
                                "The center is designed to be a refuge—a place that embodies unity through open spaces, " +
                                "natural light, and sustainable materials. Every beam, every arch speaks of our communal endeavor " +
                                "to rise above adversity. It's a project born from both creativity and a burning hope for the future."
                            );
                            communityModule.AddOption("It sounds inspiring.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            communityModule.AddOption("Were there challenges along the way?", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule challengeModule = new DialogueModule(
                                        "Oh yes—the integration of old techniques with fresh ideas was no small feat. " +
                                        "We encountered design dilemmas that tested our resolve, but Arlo's ingenuity and our " +
                                        "shared dedication transformed obstacles into stepping stones."
                                    );
                                    challengeModule.AddOption("Innovation through adversity... I see.", 
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, challengeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, communityModule));
                        });
                    arloModule.AddOption("What makes Arlo's method unique?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule methodModule = new DialogueModule(
                                "Arlo envisions structures as living entities—merging nature, art, and function. " +
                                "He advocates for renewable materials and designs that breathe with the environment. " +
                                "His creativity is so infectious that it even sparks hope during our darkest projects."
                            );
                            methodModule.AddOption("A modern, refreshing vision indeed.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, methodModule));
                        });
                    player.SendGump(new DialogueGump(player, arloModule));
                });

            // Option 4: Friendship with Syla
            greeting.AddOption("What can you tell me about your friendship with Syla?", 
                player => true,
                player =>
                {
                    DialogueModule sylaModule = new DialogueModule(
                        "Syla is the heart that inspires our collective dreams. " +
                        "Our friendship goes far beyond the realms of construction—it's a partnership of creativity " +
                        "and compassion. Together, we imagine grand spaces that serve not only as shelters but as symbols " +
                        "of hope. Would you like to hear of one of our joint visionary projects or know more about her impact on the community?"
                    );
                    sylaModule.AddOption("Tell me about one of your joint visions.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule visionModule = new DialogueModule(
                                "Imagine a vast library-workshop where knowledge and craft converge—a sanctuary " +
                                "where every stone, every scroll is imbued with the spirit of our shared aspirations. " +
                                "This vision is our way of honoring the past while building a future filled with light."
                            );
                            visionModule.AddOption("A powerful dream indeed.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, visionModule));
                        });
                    sylaModule.AddOption("What role does Syla play in your community?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule roleModule = new DialogueModule(
                                "Syla is both an artist and a leader—a unifying force in our community. " +
                                "Her innovative public art initiatives have transformed barren walls into canvases of hope, " +
                                "making her a beacon in times of uncertainty."
                            );
                            roleModule.AddOption("Her influence is truly remarkable.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, roleModule));
                        });
                    player.SendGump(new DialogueGump(player, sylaModule));
                });

            // Option 5: Secret Past and Haunted Memories
            greeting.AddOption("You seem to carry a heavy burden. What is it that haunts you, Torren?", 
                player => true,
                player =>
                {
                    DialogueModule hauntedModule = new DialogueModule(
                        "Ah... you have a keen eye. There is a darkness I rarely speak of. " +
                        "Before I took up the chisel and mortar, I was a soldier. " +
                        "I fought in terrible battles, witnessed the fall of comrades, and saw horrors that still echo in my dreams. " +
                        "I am disciplined and brave on the surface—but beneath, I am haunted by the whispers of those I have lost."
                    );
                    // Branch: talk about the visions.
                    hauntedModule.AddOption("What visions torment you?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule visionsModule = new DialogueModule(
                                "In the quiet moments, I hear their voices—the fallen soldiers of my past. " +
                                "Their pleas, regrets, and silent accusations plague me. " +
                                "At night, I see flashes of battles, broken shields, and anguished faces. " +
                                "These visions remind me of my duty to honor them by striving for perfection in all I do."
                            );
                            visionsModule.AddOption("That must be a terrible weight to bear.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, hauntedModule)));
                            visionsModule.AddOption("Do you ever find solace in your work?", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule solaceModule = new DialogueModule(
                                        "Sometimes, when I lay each stone in place, I feel as though I'm rewriting their fates—" +
                                        "transforming sorrow into legacy. The discipline of construction is my anchor, and in the " +
                                        "rhythm of my work, I chase away the ghosts of the battlefield, even if only for a fleeting moment."
                                    );
                                    solaceModule.AddOption("Your work is noble indeed.", 
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, hauntedModule)));
                                    pl2.SendGump(new DialogueGump(pl2, solaceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, visionsModule));
                        });
                    // Branch: talk about seeking battles.
                    hauntedModule.AddOption("Does your past still drive you to seek conflict?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule conflictModule = new DialogueModule(
                                "In a sense, yes. I once sought battles to escape the despair of my haunted memories, " +
                                "fighting not just enemies but my inner demons. Over time, I realized that true valor lies in building " +
                                "and healing—transforming rage and remorse into structures that endure. Yet, the call of past glories " +
                                "sometimes beckons me in the silence of the night."
                            );
                            conflictModule.AddOption("Turning conflict into creation is a brave path.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, hauntedModule)));
                            conflictModule.AddOption("Perhaps your battles now are fought with a hammer, not a sword.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule newPathModule = new DialogueModule(
                                        "Exactly. Every strike of the hammer is both a tribute to fallen comrades and a promise to " +
                                        "forgive myself. I carry my past like armor—disciplined, scarred, yet resolute to create a future " +
                                        "that heals rather than harms."
                                    );
                                    newPathModule.AddOption("A moving transformation.", 
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, hauntedModule)));
                                    pl2.SendGump(new DialogueGump(pl2, newPathModule));
                                });
                            pl.SendGump(new DialogueGump(pl, conflictModule));
                        });
                    // Branch: more on his disciplined and brave nature.
                    hauntedModule.AddOption("How do you maintain discipline with such haunting memories?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule disciplineModule = new DialogueModule(
                                "Discipline is my refuge. I adhere to a strict routine—waking with the dawn, working meticulously " +
                                "through the day, and at night, I pen the memories of battles in a worn journal. The act of recording " +
                                "my struggles keeps my mind sharp and my spirit focused. It is through discipline that I honor both the past " +
                                "and the promise of a better future."
                            );
                            disciplineModule.AddOption("Your commitment is admirable.", 
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, hauntedModule)));
                            pl.SendGump(new DialogueGump(pl, disciplineModule));
                        });
                    hauntedModule.AddOption("Thank you for trusting me with your story.", 
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, hauntedModule));
                });

            // Option 6: Advice for Aspiring Builders and Soldiers of Life
            greeting.AddOption("Do you have any advice for those chasing excellence?", 
                player => true,
                player =>
                {
                    DialogueModule adviceModule = new DialogueModule(
                        "Certainly. Whether you’re building a home, a dream, or facing the battles of life, remember: " +
                        "a strong foundation is vital. Embrace your failures, learn from the discipline of your setbacks, " +
                        "and always remain brave—even when haunted by the shadows of the past. Your craft, like your spirit, " +
                        "can be rebuilt anew with every sunrise."
                    );
                    adviceModule.AddOption("I’ll remember that—thank you.", 
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, adviceModule));
                });

            // Option 7: Exit the conversation.
            greeting.AddOption("Goodbye, Torren. May your heart find peace.", 
                player => true,
                player => player.SendMessage("Torren nods solemnly, a haunted glimmer in his eyes as he returns to his work."));

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
