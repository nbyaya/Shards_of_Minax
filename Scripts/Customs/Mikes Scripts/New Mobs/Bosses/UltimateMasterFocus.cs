using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Bruce Lee")]
    public class UltimateMasterFocus : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterFocus()
            : base(AIType.AI_Melee)
        {
            Name = "Bruce Lee";
            Title = "The Dragon";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(350, 450);
            SetDex(200, 300);
            SetInt(200, 250);

            SetHits(15000);
            SetMana(2000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Focus, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 80;
            
            AddItem(new FancyShirt(Utility.RandomYellowHue()));
            AddItem(new LongPants(Utility.RandomYellowHue()));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterFocus(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(DragonsFocus), typeof(MartialArtsManual) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(FocusScroll) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(DragonsFocus), typeof(MartialArtsManual) }; }
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

            c.DropItem(new PowerScroll(SkillName.Focus, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DragonsFocus());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MartialArtsManual());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: OneInchPunch(defender); break;
                    case 1: IronWill(); break;
                    case 2: FlurryOfBlows(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void OneInchPunch(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                int damage = Utility.RandomMinMax(100, 150);
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);
                defender.FixedParticles(0x37B9, 1, 16, 0x2A4E, 0x3F, 7, EffectLayer.Head);
                defender.PlaySound(0x1F3);
            }
        }

        public void IronWill()
        {
            this.VirtualArmorMod += 50;
            this.SendMessage("Bruce Lee's iron will increases his resistance!");
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(RemoveIronWill));
        }

        private void RemoveIronWill()
        {
            this.VirtualArmorMod -= 50;
            this.SendMessage("Bruce Lee's iron will fades.");
        }

        public void FlurryOfBlows(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                for (int i = 0; i < 5; i++)
                {
                    AOS.Damage(defender, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                }
                defender.FixedParticles(0x37B9, 1, 16, 0x2A4E, 0x3F, 7, EffectLayer.Head);
                defender.PlaySound(0x1F3);
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
