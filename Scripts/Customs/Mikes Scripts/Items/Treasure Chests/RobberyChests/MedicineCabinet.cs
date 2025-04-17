using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom
{
    public class MedicineCabinet : LockableContainer
    {
        private bool _initialized;

        [Constructable]
        public MedicineCabinet() : base(0x9A8) // Medicine Cabinet item ID
        {
            Name = "Medicine Cabinet";
            Hue = Utility.RandomMinMax(1, 1600);
            Locked = true;
            LockLevel = Utility.RandomMinMax(1, 100);
            _initialized = false; // Indicates whether items have been added
        }

        private void InitializeItems()
        {
            if (_initialized) return;

            // Add random potions, herbs, bandages, and notes
            AddItemWithProbability(new GreaterHealPotion(), 0.25);
            AddItemWithProbability(new GreaterCurePotion(), 0.25);
            AddItemWithProbability(new GreaterStrengthPotion(), 0.15);
            AddItemWithProbability(new GreaterAgilityPotion(), 0.15);
            AddItemWithProbability(new TotalRefreshPotion(), 0.10);
            AddItemWithProbability(new GreaterExplosionPotion(), 0.10);
            AddItemWithProbability(new Bandage(Utility.RandomMinMax(5, 20)), 0.50);
            AddItemWithProbability(new Garlic(Utility.RandomMinMax(5, 20)), 0.30);
            AddItemWithProbability(new Ginseng(Utility.RandomMinMax(5, 20)), 0.30);
            AddItemWithProbability(new Nightshade(Utility.RandomMinMax(5, 20)), 0.30);
            AddItemWithProbability(new MandrakeRoot(Utility.RandomMinMax(5, 20)), 0.30);
            AddItemWithProbability(new Bloodmoss(Utility.RandomMinMax(5, 20)), 0.30);
            AddItemWithProbability(new SpidersSilk(Utility.RandomMinMax(5, 20)), 0.30);
            AddItemWithProbability(CreateMedievalNote(), 0.20);
            AddItemWithProbability(CreateMedievalNote(), 0.20);
            AddItemWithProbability(CreateMedievalNote(), 0.20);
            AddItemWithProbability(CreateMedievalNote(), 0.20);
            AddItemWithProbability(CreateMedievalNote(), 0.20);
            AddItemWithProbability(new RandomFancyMedicine(), 0.05);
            AddItemWithProbability(new RandomFancyMedicine(), 0.05);
            AddItemWithProbability(new RandomFancyMedicine(), 0.05);
            AddItemWithProbability(new RandomFancyMedicine(), 0.05);
            AddItemWithProbability(new RandomFancyMedicine(), 0.05);
			AddItemWithProbability(new TeleportToTokuno(), 0.03);
			AddItemWithProbability(new TeleportToMalasItem(), 0.03);
			AddItemWithProbability(new TeleportToIlshenarItem(), 0.03);

            _initialized = true; // Mark as initialized
        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateMedievalNote()
        {
            string[] notes = new string[]
            {
                "A decoction of willow bark for pain relief.",
                "For wounds, apply a poultice of honey and crushed garlic.",
                "To soothe a cough, mix warm milk with honey and a pinch of turmeric.",
                "For indigestion, brew a tea from peppermint leaves.",
                "A potion of elderberry and honey to boost immunity.",
                "For fever, brew a tea from yarrow and elderflower.",
                "To ease headaches, apply a compress soaked in lavender water.",
                "A salve of comfrey and beeswax for joint pain.",
                "For sore throat, gargle with warm salt water.",
                "A tincture of echinacea root to ward off infections.",
                "For insomnia, drink a tea made from valerian root before bed.",
                "To reduce swelling, apply a poultice of mashed dandelion leaves.",
                "For burns, apply a salve made from aloe vera and lavender oil.",
                "A syrup of mullein and honey for respiratory ailments.",
                "For muscle aches, soak in a bath infused with rosemary and eucalyptus.",
                "To improve digestion, chew on fennel seeds after meals.",
                "A tea of chamomile and lemon balm to calm nerves.",
                "For toothache, apply clove oil to the affected area.",
                "A poultice of plantain leaves for insect bites and stings.",
                "To boost energy, drink a tonic of ginseng and honey.",
                "For skin rashes, apply a paste made from oatmeal and water.",
                "A decoction of nettle leaves to cleanse the blood.",
                "To ease menstrual cramps, drink a tea made from raspberry leaves.",
                "For cold and flu, inhale steam infused with thyme and peppermint.",
                "A balm of calendula and olive oil for dry, irritated skin.",
                "For earache, warm olive oil with a clove of garlic and apply a few drops in the ear.",
                "To strengthen hair, rinse with a tea made from rosemary and sage.",
                "A tonic of dandelion root to support liver health.",
                "For constipation, eat prunes soaked in water overnight.",
                "A bath infused with chamomile and lavender to relieve stress.",
                "For dry cough, sip on a mixture of licorice root tea and honey.",
                "To improve circulation, massage with a blend of ginger and cinnamon oils.",
                "A gargle of sage tea to alleviate mouth sores.",
                "For sunburn, apply a compress of cool black tea.",
                "A tea of lemon verbena to relieve anxiety.",
                "To clear congestion, drink a tea made from horehound leaves.",
                "For joint inflammation, apply a paste of turmeric and water.",
                "A compress of witch hazel for bruises and minor cuts.",
                "To support kidney health, drink a tea of parsley leaves.",
                "For an upset stomach, drink a tea made from ginger and lemon.",
                "A tincture of hawthorn berries to support heart health.",
                "For sore muscles, apply a liniment made from arnica and alcohol.",
                "A decoction of burdock root to purify the blood.",
                "To reduce bloating, drink a tea of dandelion and fennel.",
                "For insect repellent, apply a mixture of eucalyptus and citronella oils.",
                "A poultice of goldenseal root for infected wounds.",
                "To improve mental clarity, drink a tea made from gotu kola.",
                "For eye strain, apply a compress of chamomile tea bags.",
                "A tea of stinging nettle to alleviate allergy symptoms.",
                "For scabies, apply a paste made from neem oil and turmeric.",
				"For Dragon's Breath Fever, mix 3 dragon scales with a pinch of powdered unicorn horn. Steep in boiling water and drink thrice daily.",
				"To cure Lycanthropy, brew a potion from wolfsbane, moonstone dust, and a drop of vampire blood. Consume under a full moon.",
				"For Basilisk's Gaze, grind basilisk eyes with mandrake root and mix with purified water. Apply as eye drops every morning.",
				"To treat Phoenix Down Malady, blend phoenix feathers with essence of fire blossom and drink as a tea at sunrise.",
				"For Siren's Lure Syndrome, boil seaweed with kraken ink and a mermaid's tear. Drink the potion while singing a sea shanty.",
				"To alleviate Goblin Pox, mix goblin earwax with crushed sapphire and honey. Apply the paste to affected areas twice daily.",
				"For Chimera's Breath, brew a potion from chimera fur, dragon's blood, and a drop of griffin saliva. Drink before bed.",
				"To combat Hydra's Venom, prepare a tea from hydra scales, ginseng, and a drop of phoenix tear. Consume twice daily.",
				"For Mermaid's Curse, combine mermaid scales with sea salt and a drop of moonlight essence. Apply the mixture to the skin under the moonlight.",
				"To treat Minotaur's Madness, blend minotaur horn with valerian root and chamomile. Drink the potion to calm the mind.",
				"For Fairy's Enchantment, boil fairy dust with elderflower and a splash of morning dew. Consume the potion at dawn.",
				"To cure Kraken's Grip, mix kraken tentacle with sea kelp and a drop of siren blood. Apply the paste to the affected limbs.",
				"For Griffin's Fury, brew a potion from griffin feathers, lion's mane, and hawk's beak. Drink before facing any challenge.",
				"To treat Djinn's Curse, combine djinn's lamp oil with frankincense and a drop of desert wind. Inhale the vapors at sunset.",
				"For Wraith's Touch, mix wraith ectoplasm with lavender and a pinch of ground onyx. Apply the salve to the haunted area.",
				"To cure Troll's Warts, blend troll fat with garlic and crushed emerald. Apply the paste to the warts every night.",
				"For Elf's Insomnia, brew a tea from elfroot, lavender, and a drop of unicorn milk. Drink an hour before bed.",
				"To treat Harpy's Shriek, mix harpy feathers with thyme and honey. Gargle with the potion thrice daily.",
				"For Centaur's Hoof Ache, prepare a poultice from centaur hooves, willow bark, and peppermint. Apply to the hooves.",
				"To combat Unicorn's Ailment, brew a potion from unicorn horn, rose petals, and a drop of morning dew. Drink at dawn.",
				"For Basilisk's Bite, grind basilisk fangs with sage and a drop of dragon's blood. Apply the paste to the bite wound.",
				"To cure Vampire's Thirst, mix vampire ashes with holy water and a sprig of rosemary. Drink the potion at midnight.",
				"For Ghost's Wail, brew a potion from ghost orchid, lavender, and a drop of moonlight. Consume the potion under a full moon.",
				"To treat Golem's Rust, combine golem dust with oil of olives and a pinch of salt. Apply to the rusted areas.",
				"For Dragon's Scale Rash, mix dragon scales with aloe vera and honey. Apply the salve to the rash twice daily.",
				"To combat Griffin's Talon Ache, brew a tea from griffin talons, chamomile, and a drop of phoenix tear. Drink before bed.",
				"For Werewolf's Lament, blend werewolf fur with wolfsbane and a drop of nightshade. Consume the potion during a full moon.",
				"To treat Fairy's Flight Fatigue, mix fairy wings with ginseng and a drop of honey. Drink the potion at sunrise.",
				"For Siren's Call, boil seaweed with kraken ink and a mermaid's tear. Drink while singing a sea shanty to break the spell.",
				"To cure Basilisk's Stare, grind basilisk eyes with mandrake root and mix with purified water. Apply as eye drops every morning.",
				"For strength, drink a brew made from ground boar tusks and bitter herbs.",
				"To heal wounds, apply a paste of crushed bones and cave moss.",
				"For fever, chew on the roots of the bloodroot plant.",
				"To soothe a sore throat, gargle with a mixture of warm water and ground bat wings.",
				"For muscle pain, rub the affected area with troll fat and ground snake scales.",
				"To increase stamina, drink a potion made from dragon blood and shadow berries.",
				"For headaches, inhale the smoke from burning nightshade leaves.",
				"To improve vision, consume the eyes of a cave fish mixed with spider silk.",
				"For digestive issues, eat a mash of fermented mushrooms and wolf berries.",
				"To treat infections, apply a salve made from crushed beetles and swamp mud.",
				"For better sleep, drink a tea brewed from moonflower petals and owl feathers.",
				"To boost immune system, eat a stew of frog legs and nettle leaves.",
				"For joint pain, wrap the area with strips of bat hide soaked in sulfur water.",
				"To reduce swelling, apply a poultice of crushed bark and spider venom.",
				"For burns, cover the area with a mixture of charcoal dust and lizard scales.",
				"To stop bleeding, use a paste made from ground earthworm and moss.",
				"For toothache, chew on a mixture of bone shards and bitterroot.",
				"To enhance reflexes, drink a tonic of snake venom and cave moss.",
				"For ear infections, drop a few drops of bat oil into the ear.",
				"To clear the mind, inhale the fumes of burning sage and sulfur.",
				"For skin rashes, apply a paste made from ground insect wings and clay.",
				"To treat broken bones, wrap with strips of troll hide soaked in bone broth.",
				"For nausea, chew on dried bat wings and drink water.",
				"To increase endurance, consume a potion of ground beetles and bat blood.",
				"For eye infections, wash eyes with a mixture of cave water and bat droppings.",
				"To boost energy, eat a mix of ground dragon scales and wolf teeth.",
				"For cold symptoms, inhale steam from boiling bat wings and nightshade.",
				"To treat bites, apply a paste of ground snake fangs and swamp mud.",
				"For sore muscles, massage with a mixture of troll fat and crushed beetles.",
				"To detoxify, drink a brew made from crushed bones and nettle leaves.",
				"For anxiety, chew on the leaves of the nightshade plant.",
				"To increase aggression, consume a stew made from bear claws and wolf heart.",
				"For fungal infections, apply a paste made from ground lizard skin and clay.",
				"To improve hearing, drink a potion of bat blood and crushed snake scales.",
				"For heart health, eat a stew of cave fish and bitter herbs.",
				"To boost recovery, drink a mixture of dragon blood and spider silk.",
				"For better focus, inhale the smoke from burning sage and bat wings.",
				"To treat bruises, apply a poultice of crushed beetles and cave moss.",
				"For strength, eat the heart of a bear mixed with dragon scales.",
				"To enhance stamina, drink a tonic of troll fat and nightshade berries.",
				"For pain relief, chew on the roots of the bitterroot plant.",
				"To cure cough, drink a brew made from ground snake fangs and swamp water.",
				"For better sleep, inhale the fumes of burning sage and moonflower petals.",
				"To reduce fever, apply a paste made from ground bat wings and cave moss.",
				"For quicker healing, wrap wounds with strips of troll hide soaked in dragon blood.",
				"To improve digestion, eat a mash of fermented mushrooms and cave fish.",
				"For skin ailments, apply a salve made from crushed lizard skin and clay.",
				"To boost endurance, drink a potion of ground beetles and wolf teeth.",
				"For eye health, consume a mixture of bat blood and spider silk."
            };

            return new SimpleNote
            {
                NoteString = notes[Utility.Random(notes.Length)],
                TitleString = "Medieval Prescription"
            };
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            InitializeItems(); // Initialize items when opened for the first time
            HandlePlayerInteraction(from);
        }

		public override bool OnDragLift(Mobile from)
        {
            HandlePlayerInteraction(from);
			return base.OnDragLift(from);
        }

        public override void OnItemUsed(Mobile from, Item item)
        {
            base.OnItemUsed(from, item);
            HandlePlayerInteraction(from);
        }

        private void HandlePlayerInteraction(Mobile from)
        {
            if (from.Criminal)
            {
                from.SendMessage("You cannot interact with this container because you are flagged as a criminal.");
                return;
            }

            if (from.Hidden)
            {
                double revealChance = (1 - (from.Skills[SkillName.Hiding].Value / 200.0)); // 100 skill = 0.5 chance

                if (Utility.RandomDouble() < revealChance)
                {
                    from.RevealingAction();
                    FlagAsCriminal(from, false); // Do not call CriminalAction; just flag as criminal
                }
                else
                {
                    from.SendMessage("You successfully interact with the container while remaining hidden.");
                }
            }
            else
            {
                FlagAsCriminal(from, true); // Call CriminalAction since the player is not hidden
            }
        }

        private void FlagAsCriminal(Mobile from, bool useCriminalAction)
        {
            if (!from.Criminal)
            {
                if (useCriminalAction)
                {
                    from.CriminalAction(true); // This will flag the player and allow guards to intervene
                }
                else
                {
                    from.Criminal = true; // Only flag as criminal without guard intervention
                }
                from.SendMessage("You feel a sudden sense of guilt as you tamper with the shipping crate.");
            }
        }

        public MedicineCabinet(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(_initialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _initialized = reader.ReadBool();
        }
    }
}
