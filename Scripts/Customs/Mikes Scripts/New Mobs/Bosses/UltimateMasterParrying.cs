using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Joan of Arc")]
    public class UltimateMasterParrying : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterParrying()
            : base(AIType.AI_Melee)
        {
            Name = "Joan of Arc";
            Title = "The Maid of Orléans";
            Body = 0x191;
            Hue = 0x83F;

            SetStr(405, 525);
            SetDex(102, 150);
            SetInt(205, 350);

            SetHits(15000);
            SetMana(1500);

            SetDamage(30, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Parry, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 24000;
            Karma = 24000;

            VirtualArmor = 80;

            AddItem(new PlateChest());
            AddItem(new PlateLegs());
            AddItem(new PlateArms());
            AddItem(new PlateGloves());
            AddItem(new PlateGorget());
            AddItem(new PlateHelm());
            AddItem(new Shield());
            AddItem(new Longsword());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterParrying(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(HolyShield), typeof(SaintsArmor) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(GuardianHelm), typeof(DivineCloak) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(HolyRelic), typeof(BannerOfOrleans) }; }
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

            c.DropItem(new PowerScroll(SkillName.Parry, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new HolyShield());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new SaintsArmor());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: DivineParry(); break;
                    case 1: ShieldBash(defender); break;
                    case 2: GuardiansBlessing(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void DivineParry()
        {
            this.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            this.PlaySound(0x1FA);
            this.SendLocalizedMessage(1070840); // Divine Parry activates!
            this.VirtualArmor = 100; // Temporarily increases armor

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate
            {
                this.VirtualArmor = 80; // Reset armor after duration
                this.SendLocalizedMessage(1070841); // Divine Parry fades.
            });
        }

        public void ShieldBash(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                defender.FixedParticles(0x37B9, 1, 18, 0x26BD, 0x3F, 0x7, EffectLayer.Head);
                defender.PlaySound(0x1E0);
                defender.Paralyze(TimeSpan.FromSeconds(5.0));
                defender.SendLocalizedMessage(1070842); // You are stunned by a shield bash!
            }
        }

        public void GuardiansBlessing()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                this.DoBeneficial(m);
                m.VirtualArmorMod += 20;
                m.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist);
                m.PlaySound(0x1EA);
                m.SendLocalizedMessage(1070843); // You feel the Guardian's Blessing!

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate
                {
                    m.VirtualArmorMod -= 20;
                    m.SendLocalizedMessage(1070844); // The Guardian's Blessing fades.
                });
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
