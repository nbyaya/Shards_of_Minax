using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of an earth clan ninja")]
    public class EarthClanNinja : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // time between earth clan ninja speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public EarthClanNinja() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Setting the hue to an earthy color to fit the earth clan ninja theme
            Hue = 0x972;
			Team = 1;

            // Setting the body to human, ninjas are typically human
            Body = 0x190; // Use 0x191 for a female ninja

            Name = NameList.RandomName("male");
            Title = "the Stone Whisperer";
            
            // Dressing the ninja in an earthy ninja outfit
            AddItem(new NinjaTabi() { Hue = 0x972 }); // Earthy ninja sandals
            AddItem(new LeatherNinjaHood() { Hue = 0x972 }); // Earthy ninja hood
            AddItem(new LeatherNinjaJacket() { Hue = 0x972 }); // Earthy ninja jacket
            AddItem(new LeatherNinjaPants() { Hue = 0x972 }); // Earthy ninja pants
            AddItem(new LeatherNinjaBelt() { Hue = 0x972 }); // Earthy ninja belt

            // Defining stats for an earth clan ninja, focused on strength and resilience
            SetStr(750, 900);
            SetDex(150, 450);
            SetInt(100, 250);
            SetHits(800, 1600);

            SetDamage(10, 15);

            // Earth clan ninjas should have high stealth and tactics skills
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Fencing, 90.0, 110.0);
            SetSkill(SkillName.Macing, 90.0, 110.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Swords, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 150.0, 200.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);
            SetSkill(SkillName.Ninjitsu, 100.0, 120.0); // If your shard has the Ninjitsu skill

            // Karma for an earth clan ninja could be neutral
            Fame = 7000;
            Karma = 0;

            // Implementing earth clan ninja speech patterns, grounded and solemn
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
                        case 0: this.Say(true, "The earth will swallow you whole..."); break;
                        case 1: this.Say(true, "Feel the weight of the mountain!"); break;
                        case 2: this.Say(true, "Your strength is nothing against the stone."); break;
                        case 3: this.Say(true, "I am the unyielding rock!"); break;
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
                        case 0: this.Say(true, "The earth will swallow you whole..."); break;
                        case 1: this.Say(true, "Feel the weight of the mountain!"); break;
                        case 2: this.Say(true, "Your strength is nothing against the stone."); break;
                        case 3: this.Say(true, "I am the unyielding rock!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        // Implementing the loot generation, earth clan ninjas might carry earth-related items
        public override void GenerateLoot()
        {
            PackGold(250, 350);
            AddLoot(LootPack.UltraRich);  // Even richer loot than before
            // Add specific earth clan ninja-themed loot
            // Example: AddItem(new EarthShuriken());
        }

        public EarthClanNinja(Serial serial) : base(serial)
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