using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a fire clan samurai")]
    public class FireClanSamurai : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(25.0); // Time between fire clan samurai's speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public FireClanSamurai() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Fire Clan Samurai should have a fiery hue, representing their fire clan affiliation
            Hue = 0x489; // Adjust the hue to fit your shard's fire theme
			Team = 2;

            // Body value could remain humanoid or could be something more fiery
            Body = 0x190; // Use the shard's specific body value for samurai if available
            Name = NameList.RandomName("male");
            Title = "the Fire Clan Warrior"; // A title that denotes their role within the fire clan

            // Equip the Fire Clan Samurai with appropriate armor and weapons
            AddItem(new PlateDo() { Hue = 0x489 }); // Color the armor with fire hue
            AddItem(new PlateHaidate() { Hue = 0x489 });
            AddItem(new PlateHiroSode() { Hue = 0x489 });
            AddItem(new NinjaTabi() { Hue = 0x489 });
            AddItem(new Bascinet() { Hue = 0x489 }); // Color the helmet with fire hue
            
            // Optionally add a special fire-themed sword or other weapon
            // AddItem(new FireSword()); // Example item, replace with an actual fire-themed weapon
            
            // Set stats and skills to reflect a fierce warrior
            SetStr(700, 850);
            SetDex(200, 600);
            SetInt(100, 600);
            SetHits(700, 1400);

            SetDamage(110, 120);

            SetSkill(SkillName.Bushido, 120.0, 150.0);
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);

            // Adjust the karma and fame to reflect their fierce nature
            Fame = 15000;
            Karma = -15000; // Fire clan may be viewed as more aggressive, thus negative karma

            // Fire Clan Samurai's speech could be aggressive and fiery
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
                        case 0: this.Say(true, "Flames shall engulf you!"); break;
                        case 1: this.Say(true, "I will turn you to ash!"); break;
                        case 2: this.Say(true, "Feel the scorching wrath of my clan!"); break;
                        case 3: this.Say(true, "You will not withstand the inferno!"); break;
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
                        case 0: this.Say(true, "Your blows fuel my fiery spirit!"); break;
                        case 1: this.Say(true, "I am tempered in the flames of battle!"); break;
                        case 2: this.Say(true, "You cannot extinguish my will!"); break;
                        case 3: this.Say(true, "I am an unquenchable fire!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        // Implementing the loot generation
        public override void GenerateLoot()
        {
            // Fire Clan Samurai might carry fiery artifacts or heat-themed items
            PackGold(800, 1400);
            // Add fire-themed loot here
            // For example: PackItem(new FireRuby());
        }

        // ... Remaining methods can remain unaltered unless further customization is desired ...

        public FireClanSamurai(Serial serial) : base(serial)
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
