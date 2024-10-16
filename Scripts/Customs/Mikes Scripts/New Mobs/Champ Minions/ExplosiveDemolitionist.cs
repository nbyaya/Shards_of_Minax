using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an explosive demolitionist")]
    public class ExplosiveDemolitionist : BaseCreature
    {
        private TimeSpan m_ThrowDelay = TimeSpan.FromSeconds(15.0); // time between dynamite throws
        public DateTime m_NextThrowTime;

        [Constructable]
        public ExplosiveDemolitionist() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Demolitionist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Demolitionist";
            }

            if (Utility.RandomBool())
            {
                Item jacket = new LeatherChest();
                AddItem(jacket);
            }
            
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new Club(); // Demolitionists can carry clubs for close combat

            AddItem(hair);
            AddItem(pants);
            AddItem(boots);
            AddItem(weapon);
            weapon.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(700, 1000);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(500, 800);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 70.1, 95.0);
            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Macing, 70.1, 90.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 50;

            m_NextThrowTime = DateTime.Now + m_ThrowDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextThrowTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    this.Say(true, "Fire in the hole!");
                    ThrowDynamite(combatant);
                    m_NextThrowTime = DateTime.Now + m_ThrowDelay;
                }

                base.OnThink();
            }
        }

        public void ThrowDynamite(Mobile target)
        {
            // Logic to throw dynamite at the target's location
            // This would include creating an explosion effect and dealing damage to the target and nearby enemies
            Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 20, 10, 0, 0); // Example explosion effect
            AOS.Damage(target, this, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0); // Example damage
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
        }

        public ExplosiveDemolitionist(Serial serial) : base(serial)
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
