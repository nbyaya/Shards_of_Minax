using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of an air clan ninja")]
    public class AirClanNinja : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // quicker speech than the green ninja
        public DateTime m_NextSpeechTime;

        [Constructable]
        public AirClanNinja() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Setting the hue to a light blue to represent air
            Hue = 0x8AB;

            // Setting the body to human, ninjas are typically human
            Body = 0x190; // Use 0x191 for a female ninja

            Name = NameList.RandomName("male");
            Title = "the Whispering Gale";

            // Dressing the ninja in a blue ninja outfit
            AddItem(new NinjaTabi() { Hue = 0x8AB }); // Blue ninja sandals
            AddItem(new LeatherNinjaHood() { Hue = 0x8AB }); // Blue ninja hood
            AddItem(new LeatherNinjaJacket() { Hue = 0x8AB }); // Blue ninja jacket
            AddItem(new LeatherNinjaPants() { Hue = 0x8AB }); // Blue ninja pants
            AddItem(new LeatherNinjaBelt() { Hue = 0x8AB }); // Blue ninja belt

            // Defining stats for an air clan ninja, focused on intelligence and quick attacks
            SetStr(500, 650);
            SetDex(300, 700);
            SetInt(200, 800);
            SetHits(500, 1200);

            SetDamage(100, 120);

            // Ninjas should have high stealth and poisoning skills, but this one also has magery
            SetSkill(SkillName.EvalInt, 120.0, 200.0);
            SetSkill(SkillName.Magery, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 150.0);
            SetSkill(SkillName.Fencing, 100.0, 150.0);
            SetSkill(SkillName.Ninjitsu, 120.0, 200.0); // If your shard has the Ninjitsu skill
            SetSkill(SkillName.Stealth, 120.0, 200.0);

            // Karma for an air ninja might be positive due to their more spiritual nature
            Fame = 10000;
            Karma = 2000;

            // Implementing ninja speech patterns, with an air theme
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The wind cannot be grasped..."); break;
                        case 1: this.Say(true, "You cannot outrun the storm."); break;
                        case 2: this.Say(true, "As swift as the gust, as silent as the breeze..."); break;
                        case 3: this.Say(true, "I am the howl in the night, the whisper before the end..."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }

                base.OnThink();
            }            
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The wind cannot be grasped..."); break;
                        case 1: this.Say(true, "You cannot outrun the storm."); break;
                        case 2: this.Say(true, "As swift as the gust, as silent as the breeze..."); break;
                        case 3: this.Say(true, "I am the howl in the night, the whisper before the end..."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        // Implementing the loot generation, air clan ninjas might carry feathers, air-themed scrolls, etc.
        public override void GenerateLoot()
        {
            PackGold(250, 350);
            AddLoot(LootPack.UltraRich);  // Even richer loot than before
            // Add specific air-themed ninja loot
        }

        public AirClanNinja(Serial serial) : base(serial)
        {
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
