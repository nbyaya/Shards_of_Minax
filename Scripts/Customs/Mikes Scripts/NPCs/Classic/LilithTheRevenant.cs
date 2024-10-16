using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lilith the Revenant")]
    public class LilithTheRevenant : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilithTheRevenant() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lilith the Revenant";
            Body = 0x191; // Human female body
            Hue = 1150; // Skin color
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1155 });
            AddItem(new WizardsHat() { Hue = 1156 });
            AddItem(new Spellbook() { Name = "Lilith's Grimoire" });
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
            DialogueModule greeting = new DialogueModule("I am Lilith the Revenant, bound to this realm as a ghostly specter. Do you seek wisdom or wish to inquire about my past?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I exist in a state beyond the living, untouched by health or ailment.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My existence is now dedicated to wandering these lands as a spirit, seeking the truths of both realms. My tasks are many, yet my essence remains tethered to this plane.")));
                });

            greeting.AddOption("What do you think of the virtues?",
                player => true,
                player => {
                    DialogueModule virtuesModule = new DialogueModule("Contemplating the virtues in this ethereal existence, I ponder compassion and humility. Which of these virtues intrigues you the most?");
                    
                    virtuesModule.AddOption("Tell me about compassion.",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("Compassion is the understanding of others' suffering. It is through empathy that we can transcend our own pain and find purpose.")));
                        });

                    virtuesModule.AddOption("What about humility?",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("Humility teaches us to recognize our limitations and learn from others. It opens the door to true wisdom and understanding.")));
                        });

                    player.SendGump(new DialogueGump(player, virtuesModule));
                });

            greeting.AddOption("Do you have inquiries?",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Do you seek wisdom in the eight virtues, or do you have other inquiries? The tales of my past are woven with lessons and sorrow.")));
                });

            greeting.AddOption("What about your demise?",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My end was tragic, a consequence of love and betrayal. Would you like to hear the story of my downfall?")));
                    
                    DialogueModule demiseModule = new DialogueModule("It all began in a time of prosperity, but the shadows of deceit lingered among those I trusted. Which aspect of my story do you wish to know?");
                    
                    demiseModule.AddOption("What led to your betrayal?",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("Jealousy and greed clouded the hearts of those close to me. They plotted in the dark, and I became a pawn in their game. It shattered my world.")));
                        });

                    demiseModule.AddOption("How did you meet your end?",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("I was betrayed by a lover, struck down in an act of treachery. My spirit was bound to this realm, forced to wander until the truth is revealed.")));
                        });

                    demiseModule.AddOption("What have you learned since then?",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("In my spectral existence, I've learned that forgiveness is vital. Holding onto pain only binds us further. I seek redemption and understanding.")));
                        });

                    player.SendGump(new DialogueGump(player, demiseModule));
                });

            greeting.AddOption("Tell me about the artifacts.",
                player => true,
                player => {
                    DialogueModule artifactModule = new DialogueModule("These artifacts are remnants of past civilizations, holding secrets and powers. Do you wish to learn about a specific artifact, or would you like a tale of one?");
                    
                    artifactModule.AddOption("What is the most powerful artifact?",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("The Orb of Eternal Night is said to grant its wielder immense power, but at a great cost. It corrupts the heart of the unworthy.")));
                        });

                    artifactModule.AddOption("Can you tell me a tale?",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("Long ago, a brave warrior found a sword imbued with the essence of light. With it, he fought against the darkness that plagued our lands.")));
                        });

                    artifactModule.AddOption("What about your own artifacts?",
                        pl => true,
                        pl => {
                            TimeSpan cooldown = TimeSpan.FromMinutes(10);
                            if (DateTime.UtcNow - lastRewardTime < cooldown)
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                            }
                            else
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("I may possess an artifact that could aid you in your journey. But only if you prove worthy of its power.")));
                                player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            }
                        });

                    player.SendGump(new DialogueGump(player, artifactModule));
                });

            return greeting;
        }

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

        public LilithTheRevenant(Serial serial) : base(serial) { }
    }
}
