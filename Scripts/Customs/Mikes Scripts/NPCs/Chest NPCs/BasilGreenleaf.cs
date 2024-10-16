using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Basil Greenleaf")]
    public class BasilGreenleaf : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BasilGreenleaf() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Basil Greenleaf";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new StuddedChest() { Hue = 87 });
            AddItem(new StuddedLegs() { Hue = 87 });
            AddItem(new StuddedGloves() { Hue = 87 });
            AddItem(new Boots() { Hue = 87 });
            AddItem(new FeatheredHat() { Hue = 1157 });
            AddItem(new Pitchfork() { Hue = 1157 });
            
            Hue = Race.RandomSkinHue(); // Random skin hue
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Basil Greenleaf, guardian of the gardens.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am flourishing, thanks to the care of these marvelous plants.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to nurture and protect these gardens, ensuring they thrive.");
            }
            else if (speech.Contains("gardens"))
            {
                Say("These gardens are a place of peace and wonder. Hidden within are secrets of nature's beauty.");
            }
            else if (speech.Contains("nature"))
            {
                Say("Nature provides for us in countless ways. The more we respect it, the more it rewards us.");
            }
            else if (speech.Contains("beauty"))
            {
                Say("The beauty of nature is a reflection of its purity. It is my joy to preserve it.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The true secret of the gardens lies in understanding and harmony with nature.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is achieved when we live in balance with nature. It brings tranquility to our lives.");
            }
            else if (speech.Contains("tranquility"))
            {
                Say("Tranquility is a state of calmness and peace. It can be found in the gentle rustling of leaves and the scent of blooming flowers.");
            }
            else if (speech.Contains("leaves"))
            {
                Say("The leaves of the trees hold many stories of the forest. They whisper secrets to those who listen.");
            }
            else if (speech.Contains("whisper"))
            {
                Say("The whispers of nature can guide us if we are patient and attentive.");
            }
            else if (speech.Contains("guide"))
            {
                Say("To be guided by nature is to follow the path of wisdom and understanding.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from observing the natural world and learning from its cycles and patterns.");
            }
            else if (speech.Contains("cycles"))
            {
                Say("The cycles of nature remind us of the importance of change and renewal.");
            }
            else if (speech.Contains("renewal"))
            {
                Say("Renewal is a constant process in nature, and it should be reflected in our own lives.");
            }
            else if (speech.Contains("process"))
            {
                Say("Every process in nature is a part of a larger system, working together to maintain balance.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is key to maintaining harmony. Without it, the natural world would fall into chaos.");
            }
            else if (speech.Contains("chaos"))
            {
                Say("Chaos disrupts the natural order, but it also brings opportunities for growth and change.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth is the result of change. Embracing it leads to new beginnings and possibilities.");
            }
            else if (speech.Contains("beginnings"))
            {
                Say("Every new beginning is a chance to explore and learn more about the world around us.");
            }
            else if (speech.Contains("explore"))
            {
                Say("Exploring the natural world reveals its wonders and teaches us to appreciate its beauty.");
            }
            else if (speech.Contains("appreciate"))
            {
                Say("To truly appreciate nature, one must spend time immersed in its splendor.");
            }
            else if (speech.Contains("splendor"))
            {
                Say("The splendor of nature is all around us, from the tallest trees to the smallest flowers.");
            }
            else if (speech.Contains("flowers"))
            {
                Say("Flowers are the most delicate part of the garden, symbolizing beauty and transience.");
            }
            else if (speech.Contains("delicate"))
            {
                Say("Delicacy in nature is a reminder of the fragility of life and the need to care for it gently.");
            }
            else if (speech.Contains("fragility"))
            {
                Say("Fragility is an important aspect of life. It teaches us to handle things with care and respect.");
            }
            else if (speech.Contains("care"))
            {
                Say("Care is fundamental to maintaining the balance of nature and our own well-being.");
            }
            else if (speech.Contains("well-being"))
            {
                Say("Well-being is a state of health and contentment that comes from living in harmony with nature.");
            }
            else if (speech.Contains("contentment"))
            {
                Say("Contentment is found in appreciating the simple things in life and being grateful for what we have.");
            }
            else if (speech.Contains("simple"))
            {
                Say("Simplicity often holds the greatest beauty. Nature thrives on simplicity and elegance.");
            }
            else if (speech.Contains("elegance"))
            {
                Say("Elegance in nature is seen in its natural designs and patterns, effortlessly blending form and function.");
            }
            else if (speech.Contains("designs"))
            {
                Say("Nature's designs are both functional and beautiful, demonstrating the artistry of the natural world.");
            }
            else if (speech.Contains("artistry"))
            {
                Say("The artistry of nature is evident in its diverse forms and vibrant colors, each telling a story.");
            }
            else if (speech.Contains("diverse"))
            {
                Say("Diversity in nature ensures a rich and vibrant ecosystem, each species playing a crucial role.");
            }
            else if (speech.Contains("ecosystem"))
            {
                Say("An ecosystem is a complex network of interactions between organisms and their environment.");
            }
            else if (speech.Contains("interactions"))
            {
                Say("Interactions within an ecosystem are delicate and essential for its survival and health.");
            }
            else if (speech.Contains("survival"))
            {
                Say("Survival in nature requires adaptability and resilience, qualities that are vital for all living things.");
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is the ability to recover and adapt, a key trait for thriving in the ever-changing natural world.");
            }
            else if (speech.Contains("thrive"))
            {
                Say("To thrive is to live fully and successfully, making the most of the resources and opportunities available.");
            }
            else if (speech.Contains("resources"))
            {
                Say("Resources are the gifts of nature, providing everything needed for life and growth.");
            }
            else if (speech.Contains("gifts"))
            {
                Say("Nature's gifts are plentiful, but they must be cherished and used wisely.");
            }
            else if (speech.Contains("cherished"))
            {
                Say("Cherishing nature means respecting and protecting it for future generations to enjoy.");
            }
            else if (speech.Contains("generations"))
            {
                Say("Each generation has a responsibility to ensure the preservation and health of our natural world.");
            }
            else if (speech.Contains("preservation"))
            {
                Say("Preservation involves maintaining the integrity of natural environments and species.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity in nature is about maintaining the balance and harmony of ecosystems.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is key to maintaining the health of all living things. Without it, disorder ensues.");
            }
            else if (speech.Contains("order"))
            {
                Say("Order in nature is achieved through cycles and interactions that sustain life and promote stability.");
            }
            else if (speech.Contains("stability"))
            {
                Say("Stability provides a foundation for growth and development, allowing life to flourish.");
            }
            else if (speech.Contains("flourish"))
            {
                Say("To flourish is to reach one's full potential, a goal that both humans and nature strive for.");
            }
            else if (speech.Contains("goal"))
            {
                Say("Setting goals helps us to strive for improvement and achieve greater understanding and success.");
            }
            else if (speech.Contains("improvement"))
            {
                Say("Improvement is a continuous process of learning and adapting to create a better future.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future holds endless possibilities, shaped by the choices we make today.");
            }
            else if (speech.Contains("choices"))
            {
                Say("Our choices have a profound impact on the world around us, influencing both the present and future.");
            }
            else if (speech.Contains("impact"))
            {
                Say("The impact of our actions can be far-reaching, affecting the balance and harmony of nature.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the essence of a well-balanced life, achieved through respect and understanding of nature.");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect for nature is the foundation of a harmonious relationship with the environment.");
            }
            else if (speech.Contains("relationship"))
            {
                Say("A healthy relationship with nature fosters a sense of belonging and appreciation.");
            }
            else if (speech.Contains("belonging"))
            {
                Say("Belonging to the natural world connects us to a greater whole, reminding us of our place in it.");
            }
            else if (speech.Contains("place"))
            {
                Say("Our place in nature is both unique and interconnected with the larger ecosystem.");
            }
            else if (speech.Contains("interconnected"))
            {
                Say("Interconnectedness signifies that everything in nature is linked and dependent on one another.");
            }
            else if (speech.Contains("dependent"))
            {
                Say("Dependency in nature highlights the intricate balance required to sustain life and ensure survival.");
            }
            else if (speech.Contains("survival"))
            {
                Say("Survival is a testament to nature's resilience and adaptability, ensuring the continuation of life.");
            }
            else if (speech.Contains("continuation"))
            {
                Say("The continuation of life depends on our actions to protect and preserve the natural world.");
            }
            else if (speech.Contains("protection"))
            {
                Say("Protecting nature is a vital responsibility that ensures the well-being of all living creatures.");
            }
            else if (speech.Contains("well-being"))
            {
                Say("Well-being is achieved through a harmonious balance between human activities and the natural environment.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the key to a successful coexistence with nature, fostering a sustainable future.");
            }
            else if (speech.Contains("sustainable"))
            {
                Say("Sustainability ensures that natural resources are used wisely and responsibly, preserving them for future generations.");
            }
            else if (speech.Contains("generations"))
            {
                Say("Future generations will benefit from our commitment to sustainability and conservation efforts.");
            }
            else if (speech.Contains("conservation"))
            {
                Say("Conservation is the practice of protecting and managing natural resources to prevent depletion.");
            }
            else if (speech.Contains("depletion"))
            {
                Say("Depletion of resources can have serious consequences, underscoring the need for careful management.");
            }
            else if (speech.Contains("management"))
            {
                Say("Effective management involves balancing the needs of people and the environment to ensure long-term health.");
            }
            else if (speech.Contains("health"))
            {
                Say("The health of our planet is crucial for the well-being of all its inhabitants.");
            }
            else if (speech.Contains("inhabitants"))
            {
                Say("All inhabitants of Earth are interdependent, contributing to the rich tapestry of life.");
            }
            else if (speech.Contains("tapestry"))
            {
                Say("The tapestry of life is a beautiful and intricate pattern, woven from the diverse threads of existence.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence is a marvel, encompassing all forms of life and their interactions within the natural world.");
            }
            else if (speech.Contains("marvel"))
            {
                Say("The marvels of nature inspire awe and wonder, reminding us of the beauty and complexity of the world.");
            }
            else if (speech.Contains("wonder"))
            {
                Say("Wonder is the feeling of amazement and curiosity that drives us to explore and learn more.");
            }
            else if (speech.Contains("explore"))
            {
                Say("Exploring the natural world reveals its many secrets and deepens our appreciation for its beauty.");
            }
            else if (speech.Contains("appreciation"))
            {
                Say("Appreciation for nature fosters a deeper connection and motivates us to protect and preserve it.");
            }
            else if (speech.Contains("preservation"))
            {
                Say("Preservation is crucial for maintaining the health and vitality of natural ecosystems.");
            }
            else if (speech.Contains("ecosystems"))
            {
                Say("Ecosystems are complex networks of interactions that support life and maintain ecological balance.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance in ecosystems is essential for sustaining life and ensuring a healthy environment.");
            }
            else if (speech.Contains("environment"))
            {
                Say("The environment encompasses all living and non-living components of our planet, each playing a vital role.");
            }
            else if (speech.Contains("components"))
            {
                Say("Components of the environment include air, water, soil, and all living organisms.");
            }
            else if (speech.Contains("organisms"))
            {
                Say("Organisms interact with one another and their surroundings, contributing to the dynamic nature of ecosystems.");
            }
            else if (speech.Contains("dynamic"))
            {
                Say("The dynamic nature of ecosystems highlights their ever-changing and evolving characteristics.");
            }
            else if (speech.Contains("evolving"))
            {
                Say("Evolution is a continuous process that shapes the diversity and adaptability of life on Earth.");
            }
            else if (speech.Contains("diversity"))
            {
                Say("Diversity in ecosystems enhances resilience and promotes a more balanced and thriving environment.");
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is the ability of ecosystems to recover from disturbances and maintain functionality.");
            }
            else if (speech.Contains("functionality"))
            {
                Say("The functionality of ecosystems is vital for providing essential services and supporting life.");
            }
            else if (speech.Contains("services"))
            {
                Say("Ecosystem services include clean air, water, pollination, and many other benefits essential for survival.");
            }
            else if (speech.Contains("survival"))
            {
                Say("Survival is deeply connected to the health and sustainability of our natural environment.");
            }
            else if (speech.Contains("sustainability"))
            {
                Say("Sustainability involves using resources in a way that meets current needs without compromising future generations.");
            }
            else if (speech.Contains("generations"))
            {
                Say("Future generations will rely on our efforts to ensure a healthy and thriving planet.");
            }
            else if (speech.Contains("efforts"))
            {
                Say("Our collective efforts can make a significant difference in preserving the natural world.");
            }
            else if (speech.Contains("difference"))
            {
                Say("Every action we take can contribute to positive change and impact the health of our environment.");
            }
            else if (speech.Contains("impact"))
            {
                Say("The impact of our choices and actions shapes the future of our planet.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is shaped by our current actions and decisions, emphasizing the importance of mindful living.");
            }
            else if (speech.Contains("mindful"))
            {
                Say("Mindful living involves being aware of our actions and their effects on the world around us.");
            }
            else if (speech.Contains("awareness"))
            {
                Say("Awareness of our environment helps us make informed choices and foster a deeper connection with nature.");
            }
            else if (speech.Contains("connection"))
            {
                Say("A strong connection to nature enhances our understanding and appreciation of the world.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding nature's processes and patterns allows us to live more harmoniously with the environment.");
            }
            else if (speech.Contains("harmoniously"))
            {
                Say("Living harmoniously with nature promotes balance and well-being for all living things.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is crucial for maintaining the health and stability of ecosystems and the environment.");
            }
            else if (speech.Contains("stability"))
            {
                Say("Stability ensures that ecosystems can function properly and support a diverse range of life.");
            }
            else if (speech.Contains("support"))
            {
                Say("Supporting the health of ecosystems is essential for the well-being of all living organisms.");
            }
            else if (speech.Contains("organisms"))
            {
                Say("Organisms are interconnected within ecosystems, each playing a unique role in maintaining balance.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance in ecosystems is vital for sustaining life and ensuring a healthy environment.");
            }
            else if (speech.Contains("environment"))
            {
                Say("The environment encompasses all living and non-living components of our planet, each playing a vital role.");
            }
            else if (speech.Contains("components"))
            {
                Say("Components of the environment include air, water, soil, and all living organisms.");
            }
            else if (speech.Contains("organisms"))
            {
                Say("Organisms interact with one another and their surroundings, contributing to the dynamic nature of ecosystems.");
            }
            else if (speech.Contains("dynamic"))
            {
                Say("The dynamic nature of ecosystems highlights their ever-changing and evolving characteristics.");
            }
            else if (speech.Contains("evolving"))
            {
                Say("Evolution is a continuous process that shapes the diversity and adaptability of life on Earth.");
            }
            else if (speech.Contains("diversity"))
            {
                Say("Diversity in ecosystems enhances resilience and promotes a more balanced and thriving environment.");
            }
            else if (speech.Contains("resilience"))
            {
                Say("Resilience is the ability of ecosystems to recover from disturbances and maintain functionality.");
            }
            else if (speech.Contains("functionality"))
            {
                Say("The functionality of ecosystems is vital for providing essential services and supporting life.");
            }
            else if (speech.Contains("services"))
            {
                Say("Ecosystem services include clean air, water, pollination, and many other benefits essential for survival.");
            }
            else if (speech.Contains("survival"))
            {
                Say("Survival is deeply connected to the health and sustainability of our natural environment.");
            }
            else if (speech.Contains("sustainability"))
            {
                Say("Sustainability involves using resources in a way that meets current needs without compromising future generations.");
            }
            else if (speech.Contains("generations"))
            {
                Say("Future generations will rely on our efforts to ensure a healthy and thriving planet.");
            }
            else if (speech.Contains("efforts"))
            {
                Say("Our collective efforts can make a significant difference in preserving the natural world.");
            }
            else if (speech.Contains("difference"))
            {
                Say("Every action we take can contribute to positive change and impact the health of our environment.");
            }
            else if (speech.Contains("impact"))
            {
                Say("The impact of our choices and actions shapes the future of our planet.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is shaped by our current actions and decisions, emphasizing the importance of mindful living.");
            }
            else if (speech.Contains("mindful"))
            {
                Say("Mindful living involves being aware of our actions and their effects on the world around us.");
            }
            else if (speech.Contains("awareness"))
            {
                Say("Awareness of our environment helps us make informed choices and foster a deeper connection with nature.");
            }
            else if (speech.Contains("connection"))
            {
                Say("A strong connection to nature enhances our understanding and appreciation of the world.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding nature's processes and patterns allows us to live more harmoniously with the environment.");
            }
            else if (speech.Contains("harmoniously"))
            {
                Say("Living harmoniously with nature promotes balance and well-being for all living things.");
            }
            else if (speech.Contains("well-being"))
            {
                Say("Well-being is achieved through a harmonious balance between human activities and the natural environment.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the essence of a well-balanced life, achieved through respect and understanding of nature.");
            }
            else
            {
                Say("I'm not sure how to respond to that. Perhaps try asking me about nature or the gardens.");
            }

            base.OnSpeech(e);
        }

        public BasilGreenleaf(Serial serial) : base(serial)
        {
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
    }
}
