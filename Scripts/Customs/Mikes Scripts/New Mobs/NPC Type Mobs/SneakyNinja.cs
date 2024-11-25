using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a shadowy assailant")]
    public class SneakyNinja : BaseCreature
    {
        private TimeSpan m_HideDelay = TimeSpan.FromSeconds(20.0); // time between hide attempts
        private TimeSpan m_AttackDelay = TimeSpan.FromSeconds(5.0); // time between surprise attacks
        public DateTime m_NextHideTime;
        public DateTime m_NextAttackTime;
		public static class NinjaNames
				{
					public static readonly string[] Male = new string[]
					{
						"Jin", "Kaito", "Hanzo", "Kuro", "Raiden",
						"Taro", "Kenji", "Yasu", "Fuma", "Hayate",
						"Daichi", "Isamu", "Makoto", "Nobu", "Ryo"
					};

					public static readonly string[] Female = new string[]
					{
						"Akane", "Chiyo", "Emiko", "Fuyuko", "Harumi",
						"Kasumi", "Miyako", "Naomi", "Rin", "Sakura",
						"Takara", "Yui", "Yuna", "Reiko", "Mariko"
					};
				}

        [Constructable]
        public SneakyNinja() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            

			Hue = Utility.RandomSkinHue();
            if (Utility.RandomBool())
            {
                Body = 0x190; // Male
                Name = NinjaNames.Male[Utility.Random(NinjaNames.Male.Length)];
            }
            else
            {
                Body = 0x191; // Female
                Name = NinjaNames.Female[Utility.Random(NinjaNames.Female.Length)];
            }
			Team = Utility.RandomMinMax(1, 5);

            // Ninja outfit
            AddItem(new Kasa());
            AddItem(new NinjaTabi(Utility.RandomNeutralHue()));
            AddItem(new LeatherNinjaJacket());
            AddItem(new LeatherNinjaPants());

            // Hair
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2048)); // Various hair models
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            // Weapon
            AddItem(new Kryss() { Movable = false });

            SetStr(100, 120);
            SetDex(200, 250);
            SetInt(81, 95);

            SetHits(60, 75);

            SetDamage(5, 10);

            SetSkill(SkillName.Hiding, 80.0, 100.0);
            SetSkill(SkillName.Stealth, 80.0, 100.0);
            SetSkill(SkillName.Fencing, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Ninjitsu, 70.0, 90.0);

            Fame = 2500;
            Karma = -2500;


        }

        public override void OnThink()
        {
            base.OnThink();

            // Logic for detecting players and initiating sneak attack
            if (!Hidden && this.Combatant == null)
            {
                IPooledEnumerable eable = this.GetMobilesInRange(10); // Detection range
                foreach (Mobile m in eable)
                {
                    if (m.Player && m.InLOS(this) && m.AccessLevel == AccessLevel.Player)
                    {
                        this.Hidden = true; // Hide when a player is detected
                        break;
                    }
                }
                eable.Free();
            }
            else if (Hidden && this.Combatant != null)
            {
                this.RevealingAction(); // Reveal and attack when in combat
            }
        }
		
		public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);  // Even richer loot than before
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }

        public SneakyNinja(Serial serial) : base(serial)
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