using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a gnarrox corpse")]
    public class Gnarrox : BaseCreature
    {
        private DateTime _NextStomp;
        private DateTime _NextCharge;

        [Constructable]
        public Gnarrox()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gnarrox";
            Body = 776;
            BaseSoundID = 357;
            Hue = 0x481;

            SetStr(250, 350);
            SetDex(70, 100);
            SetInt(50, 80);

            SetHits(500, 600);
            SetDamage(12, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 60.0, 75.0);
            SetSkill(SkillName.Tactics, 70.0, 85.0);
            SetSkill(SkillName.Wrestling, 80.0, 95.0);
            SetSkill(SkillName.Anatomy, 10.0, 20.0);

            Fame = 5000;
            Karma = -5000;
            VirtualArmor = 30;

            // Standard loot
            PackItem(new Bone(Utility.RandomMinMax(5, 10)));
            PackItem(new Hides(Utility.RandomMinMax(3, 7)));

            // Pack some herbs/reagents
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
            PackItem(new Ginseng       (Utility.RandomMinMax(3,  7)));
        }

        public Gnarrox(Serial serial)
            : base(serial)
        {
        }

        public override int GetIdleSound()   { return 338; }
        public override int GetAngerSound()  { return 338; }
        public override int GetDeathSound()  { return 338; }
        public override int GetAttackSound() { return 406; }
        public override int GetHurtSound()   { return 194; }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (DateTime.UtcNow >= _NextStomp)
            {
                var targets = new List<Mobile>();
                var eable   = GetMobilesInRange(3);
                foreach (Mobile m in eable)
                {
                    if (m != this && m.Alive && CanBeHarmful(m))
                        targets.Add(m);
                }
                eable.Free();

                if (targets.Count > 0)
                {
                    Animate(AnimationType.Attack, 5);
                    PlaySound(GetAngerSound());

                    Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
                    {
                        PlaySound(0x11D);
                        Effects.SendLocationParticles(
                            EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                            Utility.RandomList(0x3709, 0x3706), 10, 30, 0x481, 0, 9502, 0
                        );

                        foreach (var target in targets)
                        {
                            if (target.Alive && CanBeHarmful(target))
                            {
                                DoHarmful(target);

                                int damage = Utility.RandomMinMax(15, 30);
                                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                                // Cast to Spell so we can call Disturb:
                                if (target.Spell is Spell spell)
                                {
                                    spell.Disturb(DisturbType.Hurt);
                                    if (target is PlayerMobile pm)
                                        pm.SendLocalizedMessage(1060497); // "Your spell has been interrupted!"
                                }

                                if (Utility.RandomDouble() < 0.25)
                                {
                                    if (target is Mobile mt)
                                    {
                                        int red = Utility.RandomMinMax(5, 10);
                                        mt.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, -red));
                                        Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                                        {
                                            mt.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, -red));
                                            if (mt is PlayerMobile)
                                                mt.SendMessage("Your physical resistance has returned to normal.");
                                        });
                                        if (mt is PlayerMobile)
                                            mt.SendMessage("The gnarrox's stomp reduces your physical resistance!");
                                    }
                                }
                            }
                        }
                    });

                    _NextStomp = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
                }
                else
                {
                    _NextStomp = DateTime.UtcNow + TimeSpan.FromSeconds(1.0);
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.30)
            {
                if (defender is Mobile target)
                {
                    switch (Utility.Random(2))
                    {
                        case 0:
                            int resistReduction = Utility.RandomMinMax(3, 8);
                            target.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, -resistReduction));
                            Timer.DelayCall(TimeSpan.FromSeconds(4.0), () =>
                            {
                                target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, -resistReduction));
                                if (target is PlayerMobile)
                                    target.SendMessage("Your physical resistance has returned to normal.");
                            });
                            if (target is PlayerMobile)
                                target.SendAsciiMessage("The gnarrox's wild charge weakens your defenses!");
                            break;
                        case 1:
                            int dexReduction = Utility.RandomMinMax(5, 10);
                            target.AddStatMod(new StatMod(StatType.Dex, "GnarroxStagger", -dexReduction, TimeSpan.FromSeconds(3.0)));
                            if (target is PlayerMobile)
                                target.SendAsciiMessage("The gnarrox's charge staggers you!");
                            break;
                    }
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != this && amount > 0 && Utility.RandomDouble() < 0.15)
            {
                int reduction = (int)(amount * Utility.RandomDouble() * 0.20);
                amount -= reduction;
                if (amount < 0) amount = 0;

                if (from is PlayerMobile)
                    from.SendAsciiMessage("The gnarrox's thick hide absorbs some of your attack!");
            }

            base.OnDamage(amount, from, willKill);
        }

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel  => Utility.RandomList(2, 3);
        public override int Meat              => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Potions);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
