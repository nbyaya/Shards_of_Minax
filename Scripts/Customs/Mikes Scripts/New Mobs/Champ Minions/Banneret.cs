using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a banneret")]
    public class Banneret : BaseCreature
    {
        private TimeSpan m_BannerEffectDelay = TimeSpan.FromSeconds(10.0); // time between banner effects
        public DateTime m_NextBannerEffectTime;

        [Constructable]
        public Banneret() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Banneret";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Banneret";
            }

            Item banner = new Item(0x1FAD); // Banner item
            banner.Hue = Utility.RandomMetalHue();
            banner.Layer = Layer.OneHanded;
            AddItem(banner);
            banner.Movable = false;

            Item armor = new PlateChest();
            armor.Hue = Utility.RandomMetalHue();
            AddItem(armor);

            Item boots = new Boots();
            boots.Hue = Utility.RandomNeutralHue();
            AddItem(boots);

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Swords, 95.5, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Parry, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextBannerEffectTime = DateTime.Now + m_BannerEffectDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBannerEffectTime)
            {
                ApplyBannerEffect();
                m_NextBannerEffectTime = DateTime.Now + m_BannerEffectDelay;
            }

            base.OnThink();
        }

        private void ApplyBannerEffect()
        {
            foreach (Mobile ally in this.GetMobilesInRange(8))
            {
                if (ally is BaseCreature && ally != this && IsAlly(ally))
                {
                    BaseCreature bc = (BaseCreature)ally;
                    bc.AddStatMod(new StatMod(StatType.Str, "BanneretStr", 10, TimeSpan.FromSeconds(10)));
                    bc.AddStatMod(new StatMod(StatType.Dex, "BanneretDex", 10, TimeSpan.FromSeconds(10)));
                    bc.AddStatMod(new StatMod(StatType.Int, "BanneretInt", 10, TimeSpan.FromSeconds(10)));

                    bc.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);
                    bc.PlaySound(0x1E0);
                }
            }
        }

        private bool IsAlly(Mobile m)
        {
            return m != null && m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster;
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public Banneret(Serial serial) : base(serial)
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
