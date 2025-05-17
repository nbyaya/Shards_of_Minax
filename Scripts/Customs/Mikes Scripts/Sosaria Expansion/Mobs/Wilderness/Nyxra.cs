using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a nyxra corpse")]
    public class Nyxra : BaseCreature
    {
        private DateTime _NextUmbralWave, _NextPounce, _NextNightsEmbrace, _NextPhaseShift;

        [Constructable]
        public Nyxra()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Nyxra";
            Body = 0xD6;
            Hue = 0x497;
            BaseSoundID = 0x462;

            SetStr(300, 400);
            SetDex(150, 250);
            SetInt(75, 125);

            SetHits(300, 400);
            SetMana(100, 200);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics,      90.0, 110.0);
            SetSkill(SkillName.Wrestling,    90.0, 110.0);
            SetSkill(SkillName.Stealth,      90.0, 110.0);
            SetSkill(SkillName.Hiding,       90.0, 110.0);

            Fame = 8000;
            Karma = -8000;
            VirtualArmor = 40;

            Tamable = false;
            ControlSlots = 0;
            MinTameSkill = 0.0;

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public Nyxra(Serial serial) : base(serial) { }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= _NextUmbralWave && InRange(target, 8) && CanBeHarmful(target) && InLOS(target))
                    PerformUmbralWave(target);

                if (DateTime.UtcNow >= _NextNightsEmbrace && InRange(target, 10) && CanBeHarmful(target) && InLOS(target))
                    PerformNightsEmbrace();
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.UtcNow >= _NextPounce && Utility.RandomDouble() < 0.2)
                PerformPounce(defender);
        }

        public void PerformUmbralWave(Mobile target)
        {
            PlaySound(0x65A);
            Animate(29, 5, 1, true, false, 0);

            _NextUmbralWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (Deleted || !Alive) return;

                PlaySound(0x208);
                this.FixedParticles(0x376A, 10, 10, Hue, 0, 0, EffectLayer.Waist);

                int range = 5;
                foreach (Mobile m in GetMobilesInRange(range))
                {
                    if (m != this && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        int raw = Utility.RandomMinMax(30, 50);
                        int dmg = raw - (m.ColdResistance * raw / 100);
                        dmg = dmg - (m.EnergyResistance * dmg / 100);
                        dmg = Math.Max(1, dmg);

                        AOS.Damage(m, this, dmg, 0, 0, 50, 0, 50);

                        var duration = TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
                        int dexRed = Utility.RandomMinMax(10, 20);
                        m.AddStatMod(new StatMod(StatType.Dex, "NyxraUmbralWaveDex", -dexRed, duration));
                        m.SendAsciiMessage("Nyxra's umbral wave chills you to the bone!");
                    }
                }
            });
        }

        public void PerformPounce(Mobile target)
        {
            _NextPounce = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            if (target == null || target.Deleted || !target.Alive) return;

            Point3D dest = target.Location;
            for (int i = 0; i < 5; i++)
            {
                var testP = new Point3D(
                    target.X + Utility.RandomMinMax(-2, 2),
                    target.Y + Utility.RandomMinMax(-2, 2),
                    target.Z
                );

                if (Map.CanFit(testP, 16, false, false) && !target.InRange(testP, 1))
                {
                    dest = testP;
                    break;
                }
            }

            if (dest != target.Location)
            {
                this.FixedParticles(0x3728, 10, 10, 0, 0, 9502, EffectLayer.Waist);
                PlaySound(0x655);

                Location = dest;

                this.FixedParticles(0x3728, 10, 10, 0, 0, 9502, EffectLayer.Waist);
                PlaySound(0x655);

                Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
                {
                    if (Deleted || !Alive || target == null || target.Deleted || !target.Alive) return;

                    Direction = GetDirectionTo(target);
                    Animate(1, 5, 1, true, false, 0);  // attack animation

                    // Extra physical damage
                    int extra = Utility.RandomMinMax(10, 20);
                    AOS.Damage(target, this, extra, 100, 0, 0, 0, 0);
                    target.SendAsciiMessage("Nyxra pounces from the shadows!");

                    target.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                    {
                        if (target != null) target.Frozen = false;
                    });
                });
            }
            else
            {
                this.FixedParticles(0x3728, 10, 10, 0, 0, 9502, EffectLayer.Waist);
                PlaySound(0x655);
            }
        }

        public void PerformNightsEmbrace()
        {
            _NextNightsEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            PlaySound(0x10);
            this.FixedParticles(0x3709, 10, 30, Hue, 0, 9904, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    var dur = TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                    int red = Utility.RandomMinMax(5, 10);

                    var strMod = m.GetStatMod("NyxraNightsEmbraceStr");
                        m.AddStatMod(new StatMod(StatType.Str, "NyxraNightsEmbraceStr", -red, dur));

                    var dexMod = m.GetStatMod("NyxraNightsEmbraceDex");
                        m.AddStatMod(new StatMod(StatType.Dex, "NyxraNightsEmbraceDex", -red, dur));

                    m.SendAsciiMessage("The shadows around Nyxra drain your strength!");
                }
            }
        }

        public void PerformPhaseShift(Mobile attacker)
        {
            _NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(60.0);

            PlaySound(0x5F8);
            this.FixedParticles(0x3728, 10, 10, 0x497, 0, 9502, EffectLayer.Waist);

            Point3D p = Location;
            bool tele = false;

            for (int i = 0; i < 10; i++)
            {
                int x = attacker.X + Utility.RandomMinMax(-10, 10);
                int y = attacker.Y + Utility.RandomMinMax(-10, 10);
                int z = Map.GetAverageZ(x, y);

                Point3D test = new Point3D(x, y, z);
                if (Map.CanFit(test, 16, false, false)
                 && !attacker.InRange(test, 5)
                 && Utility.InRange(test, Location, 5))
                {
                    p = test;
                    tele = true;
                    break;
                }
            }

            if (tele)
            {
                Location = p;
                this.FixedParticles(0x3728, 10, 10, 0x497, 0, 9502, EffectLayer.Waist);
                PlaySound(0x5F8);

                Hidden = true;
                RevealingAction();
                Combatant = null;

                Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5)), () =>
                {
                    if (!Deleted && Alive)
                        Hidden = false;
                });

                PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "*Nyxra melts into the shadows!*");
            }
            else
            {
                PlaySound(0x5F8);
                this.FixedParticles(0x3728, 10, 10, 0x497, 0, 9502, EffectLayer.Waist);
                PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "*Nyxra shimmers briefly!*");
            }
        }

        public override int Meat  => 2;
        public override int Hides => 20;
        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
        public override PackInstinct PackInstinct => PackInstinct.Feline;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls);
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
