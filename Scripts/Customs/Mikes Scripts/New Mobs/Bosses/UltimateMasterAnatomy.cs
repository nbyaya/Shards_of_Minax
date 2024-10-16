using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Leonardo da Vinci")]
    public class UltimateMasterAnatomy : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterAnatomy()
            : base(AIType.AI_Melee)
        {
            Name = "Leonardo da Vinci";
            Title = "Master of Anatomy";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(350, 475);
            SetDex(100, 200);
            SetInt(450, 650);

            SetHits(5000);
            SetMana(3000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Healing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 75;

            AddItem(new Doublet(Utility.RandomRedHue()));
            AddItem(new LongPants(Utility.RandomBlueHue()));
            AddItem(new Cloak(Utility.RandomGreenHue()));
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterAnatomy(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(AnatomistsScalpel), typeof(VitruvianAmulet) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(GreaterHealPotion), typeof(GreaterHealPotion) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(GreaterHealPotion), typeof(GreaterHealPotion) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Anatomy, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new AnatomistsScalpel());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new VitruvianAmulet());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: CriticalDissection(defender); break;
                    case 1: HealingTouch(); break;
                    case 2: VitalStrike(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void CriticalDissection(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                int damage = Utility.RandomMinMax(50, 70);
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);
                defender.SendMessage("You have been critically dissected!");
            }
        }

        public void HealingTouch()
        {
            this.Hits += Utility.RandomMinMax(100, 200);
            this.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            this.PlaySound(0x1F2);
        }

        public void VitalStrike(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                defender.Paralyze(TimeSpan.FromSeconds(3.0));
                defender.SendMessage("Your vital points have been struck, slowing you down!");
            }
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
