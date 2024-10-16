using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a weapon enchanter")]
    public class WeaponEnchanter : BaseCreature
    {
        private TimeSpan m_EnchantDelay = TimeSpan.FromSeconds(15.0); // time between enchantments
        public DateTime m_NextEnchantTime;

        [Constructable]
        public WeaponEnchanter() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Weapon Enchanter";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Weapon Enchanter";
            }

            if (Utility.RandomBool())
            {
                Item robe = new Robe();
                AddItem(robe);
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item sandals = new Sandals(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(sandals);

            SetStr(500, 700);
            SetDex(100, 150);
            SetInt(250, 300);

            SetHits(300, 400);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextEnchantTime = DateTime.Now + m_EnchantDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextEnchantTime)
            {
                foreach (Mobile ally in GetMobilesInRange(8))
                {
                    if (ally is BaseCreature && ally != this)
                    {
                        BaseCreature creature = (BaseCreature)ally;
                        if (creature.ControlMaster == this.ControlMaster)
                        {
                            creature.Say(true, "Feel the power of my enchantment!");
                            creature.Hits += 20; // Healing the ally a bit
                            creature.AddStatMod(new StatMod(StatType.Str, "EnchantStr", 10, TimeSpan.FromSeconds(30)));
                        }
                    }
                }
                m_NextEnchantTime = DateTime.Now + m_EnchantDelay;
            }
            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Rich);
        }

        public WeaponEnchanter(Serial serial) : base(serial)
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
