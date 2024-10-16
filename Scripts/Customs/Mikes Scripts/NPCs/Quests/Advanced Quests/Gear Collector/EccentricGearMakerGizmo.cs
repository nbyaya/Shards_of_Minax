using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
    public class EccentricGearMakerGizmo : BaseCreature
    {
        private Dictionary<Mobile, DateTime> _playerCooldowns = new Dictionary<Mobile, DateTime>();

        [Constructable]
        public EccentricGearMakerGizmo() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gizmo the Eccentric Gear Maker";
            Body = 400; // Human male body
            Hue = Utility.RandomSkinHue();
			
			AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Gizmo's Wacky Shirt" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000), Name = "Gizmo's Quirky Pants" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000), Name = "Eccentric Boots" });
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Gear Maker's Hat" });

            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gizmo's Gear Bag";
            AddItem(backpack);

            SetStr(60);
            SetDex(70);
            SetInt(100);

            SetHits(90);
            SetMana(120);
            SetStam(70);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 15;

        }

        public EccentricGearMakerGizmo(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            if (CanReceiveExoticGear(player))
            {
                DialogueModule greetingModule = CreateGreetingModule();
                player.SendGump(new DialogueGump(player, greetingModule));
            }
            else
            {
                TimeSpan remainingCooldown = _playerCooldowns[player] - DateTime.UtcNow;
                player.SendMessage($"You must wait {remainingCooldown.Minutes} minutes and {remainingCooldown.Seconds} seconds before talking to Gizmo again, he is very busy.");
            }
        }

        private bool CanReceiveExoticGear(PlayerMobile player)
        {
            if (_playerCooldowns.TryGetValue(player, out DateTime cooldownEnd))
            {
                if (DateTime.UtcNow >= cooldownEnd)
                {
                    _playerCooldowns[player] = DateTime.UtcNow.AddMinutes(10); // Reset cooldown for 10 minutes
                    return true;
                }
                return false;
            }

            // If the player doesn't have a cooldown, set it now and allow them to proceed.
            _playerCooldowns[player] = DateTime.UtcNow.AddMinutes(10);
            return true;
        }

		private DialogueModule CreateGreetingModule()
		{
			DialogueModule greeting = new DialogueModule("Ah, a fellow enthusiast of gears and mechanisms! I'm Gizmo, the eccentric gear maker. How can I assist you in your mechanical endeavors today?");

			// Nested dialogue explaining the options (no conditions here)
			greeting.AddOption("Tell me how I can obtain an Exotic Gear.",
				player => true,
				player =>
				{
					DialogueModule explanationModule = new DialogueModule("I have several ways you can obtain an Exotic Gear. You can buy one, exchange a rare item, craft it with alchemical knowledge, or perform a task based on your fortune.");

					// Sub-option 1: Buy Exotic Gear
					explanationModule.AddOption("Buy an Exotic Gear for 1000 Gold.",
						p => true, // No condition check here
						p =>
						{
							DialogueModule buyModule = new DialogueModule("To buy an Exotic Gear, you need 1000 Gold. Do you have the required amount?");
							
							// Check if they have the gold
							buyModule.AddOption("Yes, I have 1000 Gold.",
								pl => pl.Backpack != null && pl.Backpack.GetAmount(typeof(Gold)) >= 1000,
								pl =>
								{
									if (pl.Backpack.ConsumeTotal(typeof(Gold), 1000))
									{
										pl.AddToBackpack(new ExoticGear());
										pl.SendMessage("You have purchased an Exotic Gear for 1000 Gold.");
									}
								});

							// If they don't have enough gold
							buyModule.AddOption("No, I don't have enough gold.",
								pl => true,
								pl =>
								{
									pl.SendMessage("It seems you don't have enough Gold to make the purchase.");
									pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to main options
								});

							p.SendGump(new DialogueGump(p, buyModule)); // Show the buy option dialogue
						});

					// Sub-option 2: Exchange a Rare Cog
					explanationModule.AddOption("Exchange a Rare Cog for an Exotic Gear.",
						p => true, // No condition check here
						p =>
						{
							DialogueModule exchangeModule = new DialogueModule("You can exchange a Rare Cog for an Exotic Gear. Do you have a Rare Cog?");
							
							// Check if they have a Rare Cog
							exchangeModule.AddOption("Yes, I have a Rare Cog.",
								pl => pl.Backpack != null && pl.Backpack.GetAmount(typeof(RareCog)) >= 1,
								pl =>
								{
									if (pl.Backpack.ConsumeTotal(typeof(RareCog), 1))
									{
										pl.AddToBackpack(new ExoticGear());
										pl.SendMessage("You have exchanged a Rare Cog for an Exotic Gear.");
									}
								});

							// If they don't have a Rare Cog
							exchangeModule.AddOption("No, I don't have a Rare Cog.",
								pl => true,
								pl =>
								{
									pl.SendMessage("It seems you don't have a Rare Cog.");
									pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to main options
								});

							p.SendGump(new DialogueGump(p, exchangeModule)); // Show the exchange option dialogue
						});

					// Sub-option 3: Craft with Alchemy Skill
					explanationModule.AddOption("Craft an Exotic Gear using Alchemy.",
						p => true, // No condition check here
						p =>
						{
							DialogueModule craftModule = new DialogueModule("You need Alchemy skill (50+) and specific materials: 1 Gear Blueprint and 5 Precious Metals. Do you meet these requirements?");
							
							// Check if they meet the skill and have the items
							craftModule.AddOption("Yes, I have the skill and materials.",
								pl => pl.Skills.Alchemy.Base >= 50.0 && pl.Backpack != null && pl.Backpack.GetAmount(typeof(GearBlueprint)) >= 1 && pl.Backpack.GetAmount(typeof(PreciousMetal)) >= 5,
								pl =>
								{
									if (pl.Backpack.ConsumeTotal(typeof(GearBlueprint), 1) && pl.Backpack.ConsumeTotal(typeof(PreciousMetal), 5))
									{
										pl.AddToBackpack(new ExoticGear());
										pl.SendMessage("With your Alchemy expertise, you craft an Exotic Gear.");
									}
								});

							// If they don't meet the skill or lack the items
							craftModule.AddOption("No, I don't meet the requirements.",
								pl => true,
								pl =>
								{
									pl.SendMessage("It seems you don't have the required skill or materials.");
									pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to main options
								});

							p.SendGump(new DialogueGump(p, craftModule)); // Show the craft option dialogue
						});

					// Sub-option 4: Perform a task based on Luck
					explanationModule.AddOption("Perform a task based on your fortune.",
						p => true, // No condition check here
						p =>
						{
							DialogueModule luckModule = new DialogueModule("You need a Luck stat of at least 70 to perform this task. Do you meet the requirement?");
							
							// Check if their Luck is high enough
							luckModule.AddOption("Yes, I have at least 70 Luck.",
								pl => pl.Luck >= 70,
								pl =>
								{
									pl.AddToBackpack(new ExoticGear());
									pl.SendMessage("Your fortunate nature has earned you an Exotic Gear!");
								});

							// If they don't have enough Luck
							luckModule.AddOption("No, I don't have enough Luck.",
								pl => true,
								pl =>
								{
									pl.SendMessage("It seems you don't have enough Luck.");
									pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to main options
								});

							p.SendGump(new DialogueGump(p, luckModule)); // Show the luck option dialogue
						});

					player.SendGump(new DialogueGump(player, explanationModule)); // Show the nested explanation options
				});

			// Option to exit dialogue
			greeting.AddOption("Never mind, I don't need help right now.",
				player => true,
				player =>
				{
					player.SendMessage("Gizmo nods understandingly.");
				});

			return greeting;
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Define additional items required for conditions
    public class RareCog : Item
    {
        [Constructable]
        public RareCog() : base(0x1053) // Replace with desired item ID
        {
            Name = "Rare Cog";
            Hue = Utility.Random(1, 3000);
        }

        public RareCog(Serial serial) : base(serial)
        {
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

    public class GearBlueprint : Item
    {
        [Constructable]
        public GearBlueprint() : base(0x1C13) // Replace with desired item ID
        {
            Name = "Gear Blueprint";
            Hue = Utility.Random(1, 3000);
        }

        public GearBlueprint(Serial serial) : base(serial)
        {
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

    public class PreciousMetal : Item
    {
        [Constructable]
        public PreciousMetal() : base(0x1BEA) // Replace with desired item ID
        {
            Name = "Precious Metal";
            Hue = Utility.Random(1, 3000);
        }

        public PreciousMetal(Serial serial) : base(serial)
        {
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

    public class KarmaToken : Item
    {
        [Constructable]
        public KarmaToken() : base(0x1810) // Replace with desired item ID
        {
            Name = "Karma Token";
            Hue = Utility.Random(1, 3000);
        }

        public KarmaToken(Serial serial) : base(serial)
        {
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
